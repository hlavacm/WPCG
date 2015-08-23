using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DbExtensions;
using MySql.Data.MySqlClient;
using Netcorex.Common;
using Netcorex.Interfaces;
using Netcorex.Localizations;
using Netcorex.Models;

namespace Netcorex.ViewModels
{
	public abstract class WpViewModelBase<T> : ViewModelBase<T>, IWpViewModel
		where T : WpModelBase, new()
	{
		private readonly GenerationModel _generationModel = new GenerationModel();
		private bool _isInitialized;
		private ICommand _generateCommand;


		protected WpViewModelBase(T model, IMainContext parentContext)
			: base(model, parentContext)
		{
		}


		IWpModel IWpViewModel.Model
		{
			get { return Model; }
		}
		public GenerationModel GenerationModel
		{
			get { return _generationModel; }
		}
		public bool IsInitialized
		{
			get { return _isInitialized; }
			protected set { SetProperty(ref _isInitialized, value); }
		}
		public ICommand GenerateCommand
		{
			get { return _generateCommand ?? (_generateCommand = new RelayCommand(GenerateAction)); }
		}


		//public async void DoBusyTask(Task task, string busyContent = null)
		//{
		//	if (task == null)
		//	{
		//		return;
		//	}
		//	try
		//	{
		//		if (!string.IsNullOrEmpty(busyContent))
		//		{
		//			BusyContent = busyContent;
		//		}
		//		IsBusy = true;
		//		ParentContext.State = ApplicationStates.Working;
		//		await task;
		//	}
		//	finally
		//	{
		//		IsBusy = false;
		//		ParentContext.State = ApplicationStates.Ready;
		//	}
		//}

		public async Task InitializeAsync()
		{
			if (!IsInitialized)
			{
				try
				{
					BusyContent = Messages.Initializing;
					IsBusy = true;
					Guid taskUid = ParentContext.RunTask();
					ParentContext.WorkingMaximum += 1;
					await Task.Run(() => Initialize());
					ParentContext.WorkingValue += 1;
					ParentContext.CloseTask(taskUid);
				}
				finally
				{
					IsBusy = false;
				}
			}
		}

		public abstract bool Initialize();

		public async void GenerateAsync()
		{
			IList<long> itemsIds = GetGenerateItemsIds();
			int itemsIdsCount = itemsIds.Count;
			if (itemsIdsCount > 0)
			{
				try
				{
					BusyContent = Messages.Generating;
					IsBusy = true;
					Guid taskUid = ParentContext.RunTask();
					ParentContext.WorkingMaximum += itemsIdsCount;
					IDatabaseConnectionViewModel databaseConnectionViewModel = ParentContext.DatabaseConnectionViewModel;
					MySqlConnection dbConnection;
					if (databaseConnectionViewModel.TryGetConnection(out dbConnection))
					{
						using (dbConnection)
						{
							dbConnection.Open();
							IDatabaseConnectionModel connectionModel = databaseConnectionViewModel.Model;
							foreach (long itemId in itemsIds)
							{
								Guid subtaskUid = ParentContext.RunTask();
								await GenerateStepAsync(itemId, dbConnection, connectionModel);
								ParentContext.WorkingValue++;
								ParentContext.CloseTask(subtaskUid);
							}
						}
					}
					ParentContext.CloseTask(taskUid);
				}
				finally
				{
					IsBusy = false;
				}
			}
			else
			{
				MessageBox.Show(Messages.NoItemsSelected, Titles.Warning, MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}


		protected virtual IList<long> GetGenerateItemsIds()
		{
			return GenerationModel.GetGenerateItemsIds();
		}

		private async Task GenerateStepAsync(long itemId, MySqlConnection dbConnection, IDatabaseConnectionModel connectionModel)
		{
			await Task.Run(() => GenerateStep(itemId, dbConnection, connectionModel));
		}

		protected virtual void GenerateStep(long itemId, MySqlConnection dbConnection, IDatabaseConnectionModel connectionModel)
		{
			long lastInsertedId;
			string insertCommandText = Model.GetInsertCommandText(connectionModel);
			using (MySqlCommand command = new MySqlCommand(insertCommandText, dbConnection))
			{
				foreach (MySqlParameter parameter in Model.GetInsertCommandParameters(itemId))
				{
					command.Parameters.Add(parameter);
				}
				command.ExecuteNonQuery();
				lastInsertedId = command.LastInsertedId;
			}

			// post processing
			string insertPostProcessingCommandText = Model.GetInsertPostProcessingCommandText(connectionModel);
			if (!string.IsNullOrEmpty(insertPostProcessingCommandText))
			{
				using (MySqlCommand command = new MySqlCommand(insertPostProcessingCommandText, dbConnection))
				{
					IList<MySqlParameter> insertPostProcessingParameters = Model.GetInsertPostProcessingCommandParameters(lastInsertedId);
					if (insertPostProcessingParameters != null && insertPostProcessingParameters.Count > 0)
					{
						foreach (MySqlParameter parameter in insertPostProcessingParameters)
						{
							command.Parameters.Add(parameter);
						}
					}
					command.ExecuteNonQuery();
				}
			}
		}

		private void GenerateAction(object parameter)
		{
			GenerateAsync();
		}

		protected IDictionary<long, string> GetAvailablePosts(bool? parents = null)
		{
			IDatabaseConnectionViewModel databaseConnectionViewModel = ParentContext.DatabaseConnectionViewModel;
			SqlBuilder sqlBuilder = new SqlBuilder();
			sqlBuilder.SELECT("ID, post_title, post_type");
			sqlBuilder.FROM(databaseConnectionViewModel.Model.GetTablePrefixed(PostModel.TableName));
			sqlBuilder.WHERE("post_status = 'publish' AND post_title != ''");
			if (parents == true)
			{
				sqlBuilder.WHERE("AND post_parent = 0");
			}
			else
				if (parents == false)
			{
				sqlBuilder.WHERE("AND post_parent != 0");
			}
			sqlBuilder.ORDER_BY("post_title");
			Func<MySqlDataReader, long> processKeyFunc = reader => GetReaderKey(reader, "ID");
			Func<MySqlDataReader, string> processValueFunc = reader => GetReaderValue(reader, "post_title", "post_type");
			return GetAvailableData(sqlBuilder.ToString(), processKeyFunc, processValueFunc);
		}

		protected IList<string> GetAvailablePostTypes()
		{
			IDatabaseConnectionViewModel databaseConnectionViewModel = ParentContext.DatabaseConnectionViewModel;
			SqlBuilder sqlBuilder = new SqlBuilder();
			sqlBuilder.SELECT("post_type");
			sqlBuilder.FROM(databaseConnectionViewModel.Model.GetTablePrefixed(PostModel.TableName));
			sqlBuilder.GROUP_BY("post_type");
			sqlBuilder.ORDER_BY("post_type");
			Func<MySqlDataReader, string> processValueFunc = reader => GetReaderValue(reader, "post_type");
			return GetAvailableList(sqlBuilder.ToString(), processValueFunc);
		}

		protected IDictionary<long, string> GetAvailableTaxonomies(bool? parents = null, bool withCounts = true)
		{
			IDatabaseConnectionViewModel databaseConnectionViewModel = ParentContext.DatabaseConnectionViewModel;
			SqlBuilder sqlBuilder = new SqlBuilder();
			sqlBuilder.SELECT((withCounts) ? "term_taxonomy_id, taxonomy, count" : "term_taxonomy_id, taxonomy");
			sqlBuilder.FROM(databaseConnectionViewModel.Model.GetTablePrefixed(TaxonomyModel.TableName));
			if (parents == true)
			{
				sqlBuilder.WHERE("parent = 0");
			}
			else
				if (parents == false)
			{
				sqlBuilder.WHERE("parent != 0");
			}
			sqlBuilder.GROUP_BY("taxonomy");
			sqlBuilder.ORDER_BY("taxonomy");
			Func<MySqlDataReader, long> processKeyFunc = reader => GetReaderKey(reader, "term_taxonomy_id");
			Func<MySqlDataReader, string> processValueFunc = reader => GetReaderValue(reader, "taxonomy", (withCounts) ? "count" : null);
			return GetAvailableData(sqlBuilder.ToString(), processKeyFunc, processValueFunc);
		}

		protected IDictionary<long, string> GetAvailableTermTaxonomies(bool? parents = null, bool withCounts = true)
		{
			IDatabaseConnectionViewModel databaseConnectionViewModel = ParentContext.DatabaseConnectionViewModel;
			SqlBuilder sqlBuilder = new SqlBuilder();
			sqlBuilder.SELECT((withCounts) ? "term_taxonomy_id, name, count" : "term_taxonomy_id, name");
			sqlBuilder.FROM(databaseConnectionViewModel.Model.GetTablePrefixed(TaxonomyModel.TableName));
			sqlBuilder.JOIN(string.Format("wp_terms ON {0}.term_id = {1}.term_id", databaseConnectionViewModel.Model.GetTablePrefixed(TaxonomyModel.TableName), databaseConnectionViewModel.Model.GetTablePrefixed(TermModel.TableName)));
			if (parents == true)
			{
				sqlBuilder.WHERE("parent = 0");
			}
			else
				if (parents == false)
			{
				sqlBuilder.WHERE("parent != 0");
			}
			sqlBuilder.GROUP_BY("name");
			sqlBuilder.ORDER_BY("name");
			Func<MySqlDataReader, long> processKeyFunc = reader => GetReaderKey(reader, "term_taxonomy_id");
			Func<MySqlDataReader, string> processValueFunc = reader => GetReaderValue(reader, "name", (withCounts) ? "count" : null);
			return GetAvailableData(sqlBuilder.ToString(), processKeyFunc, processValueFunc);
		}

		protected IDictionary<long, string> GetAvailableTerms()
		{
			IDatabaseConnectionViewModel databaseConnectionViewModel = ParentContext.DatabaseConnectionViewModel;
			SqlBuilder sqlBuilder = new SqlBuilder();
			sqlBuilder.SELECT("wp_terms.term_id AS ID, name, taxonomy");
			sqlBuilder.FROM(databaseConnectionViewModel.Model.GetTablePrefixed(TaxonomyModel.TableName));
			sqlBuilder.JOIN(string.Format("wp_terms ON {0}.term_id = {1}.term_id", databaseConnectionViewModel.Model.GetTablePrefixed(TaxonomyModel.TableName), databaseConnectionViewModel.Model.GetTablePrefixed(TermModel.TableName)));
			sqlBuilder.GROUP_BY("name");
			sqlBuilder.ORDER_BY("name");
			Func<MySqlDataReader, long> processKeyFunc = reader => GetReaderKey(reader, "ID");
			Func<MySqlDataReader, string> processValueFunc = reader => GetReaderValue(reader, "name", "taxonomy");
			return GetAvailableData(sqlBuilder.ToString(), processKeyFunc, processValueFunc);
		}

		protected IDictionary<long, string> GetAvailableAuthors()
		{
			IDatabaseConnectionViewModel databaseConnectionViewModel = ParentContext.DatabaseConnectionViewModel;
			SqlBuilder sqlBuilder = new SqlBuilder();
			sqlBuilder.SELECT("ID, display_name");
			sqlBuilder.FROM(databaseConnectionViewModel.Model.GetTablePrefixed(UserModel.TableName));
			sqlBuilder.ORDER_BY("display_name");
			Func<MySqlDataReader, long> processKeyFunc = reader => GetReaderKey(reader, "ID");
			Func<MySqlDataReader, string> processValueFunc = reader => GetReaderValue(reader, "display_name");
			return GetAvailableData(sqlBuilder.ToString(), processKeyFunc, processValueFunc);
		}

		private IList<string> GetAvailableList(string commandText, Func<MySqlDataReader, string> processValueFunc)
		{
			if (string.IsNullOrEmpty(commandText))
			{
				throw new ArgumentNullException("commandText");
			}
			if (processValueFunc == null)
			{
				throw new ArgumentNullException("processValueFunc");
			}
			IDatabaseConnectionViewModel databaseConnectionViewModel = ParentContext.DatabaseConnectionViewModel;
			MySqlConnection connection;
			if (databaseConnectionViewModel != null && databaseConnectionViewModel.TryGetConnection(out connection))
			{
				using (connection)
				{
					connection.Open();
					using (MySqlCommand command = new MySqlCommand(commandText, connection))
					{
						using (MySqlDataReader reader = command.ExecuteReader())
						{
							IList<string> list = new ObservableCollection<string>();
							while (reader.Read())
							{
								list.Add(processValueFunc(reader));
							}
							return list;
						}
					}
				}
			}
			return null;
		}

		private IDictionary<long, string> GetAvailableData(string commandText, Func<MySqlDataReader, long> processKeyFunc, Func<MySqlDataReader, string> processValueFunc)
		{
			if (string.IsNullOrEmpty(commandText))
			{
				throw new ArgumentNullException("commandText");
			}
			if (processKeyFunc == null)
			{
				throw new ArgumentNullException("processKeyFunc");
			}
			if (processValueFunc == null)
			{
				throw new ArgumentNullException("processValueFunc");
			}
			IDatabaseConnectionViewModel databaseConnectionViewModel = ParentContext.DatabaseConnectionViewModel;
			MySqlConnection connection;
			if (databaseConnectionViewModel != null && databaseConnectionViewModel.TryGetConnection(out connection))
			{
				using (connection)
				{
					connection.Open();
					using (MySqlCommand command = new MySqlCommand(commandText, connection))
					{
						using (MySqlDataReader reader = command.ExecuteReader())
						{
							IDictionary<long, string> data = new Dictionary<long, string>();
							while (reader.Read())
							{
								data.Add(processKeyFunc(reader), processValueFunc(reader));
							}
							return data;
						}
					}
				}
			}
			return null;
		}

		private long GetReaderKey(MySqlDataReader reader, string idColumnName)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (string.IsNullOrEmpty(idColumnName))
			{
				throw new ArgumentNullException("idColumnName");
			}
			int id = reader.GetInt32(idColumnName);
			return id;
		}

		private string GetReaderValue(MySqlDataReader reader, string titleColumnName, string additionalColumnName = null)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (string.IsNullOrEmpty(titleColumnName))
			{
				throw new ArgumentNullException("titleColumnName");
			}
			string title = reader.GetString(titleColumnName);
			if (string.IsNullOrEmpty(additionalColumnName))
			{
				return title;
			}
			string additional = reader.GetString(additionalColumnName);
			return string.Format("{0} ({1})", title, additional);
		}
	}
}
