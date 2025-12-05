// IAsyncState.cs
using System.Collections;

namespace MyStateMachine
{
    /// <summary>
    /// Optional interface for states that want async enter/exit using coroutines.
    /// </summary>
    public interface IAsyncState
    {
        IEnumerator EnterAsync();
        IEnumerator ExitAsync();
    }
}
