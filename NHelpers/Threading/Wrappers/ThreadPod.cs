namespace EasySharp.NHelpers.Threading.Wrappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;

    /// <summary>
    ///     <para>Contains the <see cref="Expression{TDelegate}" /> to be executed in a separate thread.</para>
    ///     <para>Contains the execution order dependencies on other threads.</para>
    ///     <para>
    ///         Contains the execution order dependencies of other threads that are waiting for the current thread
    ///         completion.
    ///     </para>
    /// </summary>
    public class ThreadPod : IDisposable
    {
        private readonly Expression<Action> _threadActionExpression;
        public int ThreadWrapperIdentifier { get; }
        public int DependenciesNumber { get; }
        public CountdownEvent PersonalCountdownEvent { get; }
        public List<CountdownEvent> InjectedCountdownEventsList { get; }
        public int[] WaitForThreads { get; }

        #region CONSTRUCTORS

        public ThreadPod(int dependenciesNumber, Expression<Action> threadActionExpression, int[] waitForThreads)
            : this(
                threadWrapperIdentifier: -1,
                dependenciesNumber: dependenciesNumber,
                threadActionExpression: threadActionExpression,
                waitForThreads: waitForThreads) { }

        public ThreadPod(int threadWrapperIdentifier, int dependenciesNumber,
            Expression<Action> threadActionExpression,
            int[] waitForThreads)
        {
            ThreadWrapperIdentifier = threadWrapperIdentifier;

            DependenciesNumber = dependenciesNumber;
            _threadActionExpression = threadActionExpression;

            if (waitForThreads.Length > 0)
                PersonalCountdownEvent = new CountdownEvent(waitForThreads.Length);

            WaitForThreads = waitForThreads;
            InjectedCountdownEventsList = new List<CountdownEvent>();
        }

        #endregion

        public void Dispose() => PersonalCountdownEvent?.Dispose();

        /// <summary>
        ///     Injects <see cref="CountdownEvent" />s, thus imposing the execution order dependencies to threads that are waiting
        ///     for the current thread completion.
        /// </summary>
        /// <param name="countdownEvent"></param>
        public void InjectCountsownEvent(CountdownEvent countdownEvent)
        {
            InjectedCountdownEventsList.Add(countdownEvent);
        }

        /// <summary>
        ///     <para>Builds <see cref="Expression{Action}" /> and runs the <see cref="Action" /> in a separate thread.</para>
        /// </summary>
        /// <remarks>
        ///     After the <see cref="Action" /> is completed, this method signalizes other threads that are waiting
        ///     for the current thread completion.
        /// </remarks>
        public void Run()
        {
            new Thread(() =>
            {
                PersonalCountdownEvent?.Wait();

                _threadActionExpression?.Compile().Invoke();

                foreach (var countdownEvent in InjectedCountdownEventsList)
                {
                    countdownEvent.Signal();
                }
            }).Start();
        }
    }
}