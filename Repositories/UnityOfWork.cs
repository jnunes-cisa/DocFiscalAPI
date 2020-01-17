using System;

namespace Repositories
{
    public class UnityOfWork : IDisposable
    {
        #region Privates

        private BaseContext _baseContext = null;

        private bool _disposed = false;

        #endregion

        #region Public Props

        

        #endregion

        public UnityOfWork(BaseContext baseContext)
        {
            this._baseContext = baseContext;
        }

        public int SaveAllChanges()
        {

            return this._baseContext.SaveChanges();

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
                if (disposing)
                    this._baseContext.Dispose();

            this._disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}