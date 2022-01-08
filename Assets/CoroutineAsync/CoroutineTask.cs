using System;
using System.Collections;
using UnityEngine;

namespace CoroutineAsync
{
    public class CoroutineTask
    {
        private Coroutine _coroutine;
        private IEnumerator _routine;
        private bool _isCompleted;

        public bool IsCompleted => _isCompleted;

        public static Coroutine Delay(int milliseconds)
            => new CoroutineTask(DelayCoroutine(milliseconds));

        public static Coroutine Run(IEnumerator routine) => new CoroutineTask(routine);
        public static Coroutine Run(Func<IEnumerator> funcRoutine) => new CoroutineTask(funcRoutine.Invoke());

        private CoroutineTask(IEnumerator routine)
        {
            _routine = routine;
            _coroutine = StaticCoroutine.StartCoroutine(Routine());
        }

        private IEnumerator Routine()
        {
            while (_routine.MoveNext())
                yield return _routine.Current;
            _isCompleted = true;
        }

        public static implicit operator Coroutine(CoroutineTask coroutineTask)
        {
            return coroutineTask._coroutine;
        }

        private static IEnumerator DelayCoroutine(int milliseconds)
        {
            yield return new WaitForSeconds((float) milliseconds / 1000);
        }
    }

    public class CoroutineTask<T> where T : class
    {
        private Coroutine _coroutine;
        private IEnumerator _routine;
        private object _result;
        private bool _isCompleted;

        public T Result => _result as T;
        public bool IsCompleted => _isCompleted;

        public static Coroutine Run(IEnumerator routine, out CoroutineTask<T> task)
            => task = new CoroutineTask<T>(routine);

        public static Coroutine Run(Func<IEnumerator> routineFunc, out CoroutineTask<T> task)
            => Run(routineFunc.Invoke, out task);

        private CoroutineTask(IEnumerator routine)
        {
            _routine = routine;
            _coroutine = StaticCoroutine.StartCoroutine(Routine());
        }

        private IEnumerator Routine()
        {
            while (_routine.MoveNext())
            {
                _result = _routine.Current;
                yield return _result;
            }

            _isCompleted = true;
        }

        public static implicit operator Coroutine(CoroutineTask<T> coroutineTask)
        {
            return coroutineTask._coroutine;
        }
    }
}