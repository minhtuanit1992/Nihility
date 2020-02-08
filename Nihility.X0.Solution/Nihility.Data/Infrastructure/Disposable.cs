using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nihility.Data.Infrastructure
{
    public class Disposable : IDisposable
    {
        private bool isDisposed;

        ~Disposable()
        {

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!isDisposed && disposing)
            {
                DisposeCore();
            }
            isDisposed = true;
        }

        // Ghi đè phương thức này để tùy chỉnh đối tượng cần Dispose
        protected virtual void DisposeCore()
        {

        }
    }
}
