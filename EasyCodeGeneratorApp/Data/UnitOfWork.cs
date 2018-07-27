using System;
using System.Data;

namespace EasyCodeGeneratorApp.Data
{
    public interface IUnitOfWork : IDisposable
    {
        int? CurrentId { get; set; }
        string CurrentName { get; set; }
        IDbConnection Connection { get; set; }
        IDbTransaction Transaction { get; set; }
        int? CommandTimeout { get; set; }
        void Commit();
        void Rollback();
    }
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IDbConnection connection, string connStr)
        {
            Connection = connection;
            Connection.ConnectionString = connStr;
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
            //CommandTimeout = default(int?);
           
            //Transaction = Connection.BeginTransaction();//IsolationLevel.ReadUncommitted

            //IsCommit = false;
        }

        public int? CurrentId { get; set; }
        public string CurrentName { get; set; }
        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }
        private bool IsCommit { get; set; }
        public int? CommandTimeout { get; set; }

        public void Commit()
        {
            //if (!IsCommit && Connection.State == ConnectionState.Open)
            //{
            //    this.Transaction.Commit();
            //    IsCommit = true;
            //}
        }

        public void Rollback()
        {
            //if (!IsCommit)
            //{
            //    this.Transaction.Rollback();
            //    IsCommit = true;
            //}
        }

        public void Dispose()
        {
            //try
            //{
            //    Commit();
            //}
            //catch
            //{
            //    Rollback();
            //}
            //finally
            //{
            //    Connection.Close();
            //}
        }
    }
}
