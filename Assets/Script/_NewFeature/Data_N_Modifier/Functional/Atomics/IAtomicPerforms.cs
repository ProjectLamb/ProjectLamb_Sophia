namespace Sophia {
    public interface IInvokeOnStartPerform<T> {
        public void Invoke(ref T input);
    }
    public interface IRunPerform<T> {
        public void Run(ref T input);
    }

    public interface IExitPerform <T> {
        public void Exit(ref T input);
    }
}