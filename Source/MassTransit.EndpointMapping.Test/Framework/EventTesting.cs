using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Humanizer;
using NUnit.Framework;

namespace MassTransit.EndpointMapping.Test.Framework
{
    public class EventTesting
    {
        private static readonly Subject<object> Events = new Subject<object>();
        private static readonly TimeSpan TimeoutPeriod = 20.Seconds();
        private readonly List<Task> _expectedEventChecks = new List<Task>();

        [TearDown]
        public void TearDown()
        {
            _expectedEventChecks.Clear();
        }

        public static void ReportEvent(object @event)
        {
            Events.OnNext(@event);
        }

        protected void SetEventExpectation<TEvent>(Func<TEvent, bool> predicate)
        {
            var expectedEventCheck =
                Events
                    .Where(x => x is TEvent)
                    .Cast<TEvent>()
                    .Where(predicate)
                    .Timeout(TimeoutPeriod,
                        Observable.Throw<TEvent>(
                            new TimeoutException("Timed out waiting to satisfy event expectation of type " +
                                                 typeof(TEvent).FullName)))
                    .Take(1)
                    .Select(_ => Unit.Default)
                    .ToTask();

            _expectedEventChecks.Add(expectedEventCheck);
        }

        protected void VerifyEventExpectations()
        {
            Task.WaitAll(_expectedEventChecks.ToArray());
        }
    }
}