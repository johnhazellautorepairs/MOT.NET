using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MOT.NET {
    internal class SecureStringReader : IDisposable {
        private IntPtr _ptr;
        private string _string;

        internal SecureStringReader(SecureString ss) {
            _ptr = Marshal.SecureStringToGlobalAllocUnicode(ss);
            _string = Marshal.PtrToStringUni(_ptr);
        }

        public override string ToString() {
            return _string;
        }

        #region IDisposable Support
        private bool disposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposed) {
                if (disposing) {}
                Marshal.ZeroFreeBSTR(_ptr);
                disposed = true;
            }
        }

        ~SecureStringReader() {
          Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}