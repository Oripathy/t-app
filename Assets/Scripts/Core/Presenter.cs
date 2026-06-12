using System;

namespace Core
{
    public abstract class Presenter<TView> : IDisposable
        where TView : View
    {
        protected TView View { get; private set; }
        
        public virtual void SetView(TView view)
        {
            View = view;
        }

        public virtual void Dispose()
        {
            
        }
    }

    public abstract class Presenter<TModel, TView> : Presenter<TView>
        where TView : View
    {
        protected TModel Model { get; }

        public Presenter(TModel model)
        {
            Model = model;
        }
    }
}