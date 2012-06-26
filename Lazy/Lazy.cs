using System;

namespace Emkay.Lazy
{
    public class Simple<T> where T : class, new()
    {
        protected T _instance;

        public virtual bool IsInstanciated
        {
            get
            {
                return null != _instance;
            }
        }

        public virtual T Instance
        {
            get { return _instance ?? (_instance = new T()); }
        }
    }

    public class SimpleThreadSafe<T> : Simple<T> where T : class, new()
    {
        public override bool IsInstanciated
        {
            get
            {
                if (null == _instance)
                {
                    lock (this)
                    {
                        return null == _instance;
                    }
                }
                return true;
            }
        }

        public override T Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (this)
                    {
                        if (null == _instance)
                        {
                            _instance = new T();
                        }
                    }
                }
                return _instance;
            }
        }
    }

    public class Enhanced<T> where T : class
    {
        protected T _instance;

        public delegate T Factory();

        protected readonly Factory _factory;

        public virtual bool IsInstanciated
        {
            get { return null != _instance; }
        }

        public virtual T Instance
        {
            get { return _instance ?? (_instance = _factory()); }
        }

        public Enhanced(Factory factory)
        {
            if (null == factory)
            {
                throw new ArgumentNullException("factory");
            }
            _factory = factory;
        }
    }

    public class EnhancedThreadSafe<T> : Enhanced<T> where T : class
    {
        public override bool IsInstanciated
        {
            get
            {
                if (null == _instance)
                {
                    lock (this)
                    {
                        return null == _instance;
                    }
                }
                return true;
            }
        }

        public override T Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (this)
                    {
                        if (null == _instance)
                        {
                            _instance = _factory();
                        }
                    }
                }
                return _instance;
            }
        }

        public EnhancedThreadSafe(Factory factory) : base(factory) {}
    }
}
