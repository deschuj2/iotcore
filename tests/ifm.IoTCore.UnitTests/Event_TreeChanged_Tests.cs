namespace ifm.IoTCore.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Common.Exceptions;
    using ElementManager.Contracts.Elements;
    using ElementManager.Contracts.Elements.Tree;
    using Factory;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable(ParallelScope.None)]
    public class Event_TreeChanged_Tests
    {
        IIoTCore testiotcore;
        IBaseElement sender; TreeChangedEventArgs<IBaseElement> treechanged;
        ManualResetEventSlim TreeChangedDone;
        const int TreeChangedTimeoutms = 100; // milliseconds
        const string TreeChangedTimeoutMessage = "TreeChangedEvent not triggered before wait timeout";

        void CopyEventArgs(object s, TreeChangedEventArgs<IBaseElement> tce)
        {
            sender = s as IBaseElement;
            treechanged = tce;
            TreeChangedDone?.Set();
        }

        [OneTimeSetUp]
        public void BeforeAll_TreeChangedTests()
        {
            testiotcore = IoTCoreFactory.Create("testiotcore");
            testiotcore.Root.TreeChanged += CopyEventArgs;
            TreeChangedDone = new ManualResetEventSlim();
        }

        [SetUp]
        public void BeforeEach_TreechangedTests()
        {
            sender = null;
            treechanged = null;
            TreeChangedDone.Reset();
        }

        [OneTimeTearDown]
        public void AfterAll_TreeChangedTests()
        {
            testiotcore.Root.TreeChanged -= CopyEventArgs;
            testiotcore.Dispose();
        }

        [Test, Property("TestCaseKey", "IOTCS-T17")]
        public void TreeChangedEvent_Trigger_OnElementCreation()
        {
            testiotcore.ElementManager.CreateStructureElement(testiotcore.Root, Guid.NewGuid().ToString("N"), raiseTreeChanged:true);
            TreeChangedDone.Wait(TreeChangedTimeoutms);
            Assert.That(TreeChangedDone.IsSet, TreeChangedTimeoutMessage);
        }

        [Test, Property("TestCaseKey", "IOTCS-T17")]
        public void TreeChangedEvent_Trigger_OnElementCreation_Supressable()
        {
            testiotcore.ElementManager.CreateStructureElement(testiotcore.Root, Guid.NewGuid().ToString("N"), raiseTreeChanged:false);
            TreeChangedDone.Wait(TreeChangedTimeoutms);
            Assert.That(TreeChangedDone.IsSet, Is.False, "expected no TreeChangedEvent trigger");
        }

        [Test, Property("TestCaseKey", "IOTCS-T17")]
        public void TreeChangedEvent_Trigger_ElementAdded_OnCreateElement()
        {
            testiotcore.ElementManager.CreateStructureElement(testiotcore.Root, Guid.NewGuid().ToString("N"), raiseTreeChanged:true);
            TreeChangedDone.Wait(TreeChangedTimeoutms);
            Assert.That(TreeChangedDone.IsSet,TreeChangedTimeoutMessage);
            Assert.That(treechanged.Action, Is.EqualTo(TreeChangedActions.ChildAdded));
        }

        [Test, Property("TestCaseKey", "IOTCS-T17")]
        public void TreeChangedEvent_Trigger_ElementRemoved_OnRemoveElement()
        {
            var element = testiotcore.ElementManager.CreateStructureElement(testiotcore.Root, Guid.NewGuid().ToString("N"));

            testiotcore.ElementManager.RemoveElement(testiotcore.Root, element, true);
            TreeChangedDone.Wait(TreeChangedTimeoutms);
            Assert.That(TreeChangedDone.IsSet,TreeChangedTimeoutMessage);
            Assert.That(treechanged.Action, Is.EqualTo(TreeChangedActions.ChildRemoved));
        }

        [Test, Property("TestCaseKey", "IOTCS-T17")]
        public void TreeChangedEvent_Trigger_OnRaiseTreeChanged_ElementAdded()
        {
            testiotcore.ElementManager.RaiseTreeChanged(TreeChangedActions.ChildAdded, testiotcore.Root, null);
            TreeChangedDone.Wait(TreeChangedTimeoutms);
            Assert.That(TreeChangedDone.IsSet, TreeChangedTimeoutMessage);
            Assert.That(treechanged.Action, Is.EqualTo(TreeChangedActions.ChildAdded));
        }

        [Test, Property("TestCaseKey", "IOTCS-T17")]
        public void TreeChangedEvent_Trigger_OnRaiseTreeChanged_ElementRemoved()
        {
            testiotcore.ElementManager.RaiseTreeChanged(TreeChangedActions.ChildRemoved, testiotcore.Root, null);
            TreeChangedDone.Wait(TreeChangedTimeoutms);
            Assert.That(TreeChangedDone.IsSet, TreeChangedTimeoutMessage);
            Assert.That(treechanged.Action, Is.EqualTo(TreeChangedActions.ChildRemoved));
        }

        [Test, Property("TestCaseKey", "IOTCS-T17")]
        public void TreeChangedEvent_Trigger_OnRaiseTreeChanged_TreeChanged()
        {
            testiotcore.ElementManager.RaiseTreeChanged(TreeChangedActions.TreeChanged, testiotcore.Root,null);
            TreeChangedDone.Wait(TreeChangedTimeoutms);
            Assert.That(TreeChangedDone.IsSet, TreeChangedTimeoutMessage);
            Assert.That(treechanged.Action, Is.EqualTo(TreeChangedActions.TreeChanged));
        }

        [Test, Property("TestCaseKey", "IOTCS-T17")]
        public void TreeChangedEvent_MultipleHandlers1000()
        {
            const int MaxHandlers = 1000;
            using (var ioTCore = IoTCoreFactory.Create("ioTCore"))
            {
                var handled = new List<bool>();

                void OnTreeChanged(object sender, TreeChangedEventArgs<IBaseElement> eventargs)
                {
                    handled.Add(true);
                }

                // add multiple handlers to TreeChanged Event 
                for (var i = 0; i < MaxHandlers; i++)
                {
                    ioTCore.Root.TreeChanged += OnTreeChanged;
                }

                // create an element for add / remove
                ioTCore.ElementManager.CreateStructureElement(ioTCore.Root, "struct0", raiseTreeChanged: true);

                for (var i = 0; i < MaxHandlers; i++)
                {
                    ioTCore.Root.TreeChanged -= OnTreeChanged;
                }

                // check Add child with trigger
                Assert.That(handled.Count, Is.EqualTo(MaxHandlers));
                Assert.That(handled, Has.All.True);
            }
        }

        [Test, Property("TestCaseKey", "IOTCS-T17")]
        public void TreeChangedEvent_Triggered_Not_OnException()
        {
            using (var testiotcore = IoTCoreFactory.Create("myIot"))
            {
                var triggered = false;
                var struct0 = testiotcore.ElementManager.CreateStructureElement(testiotcore.Root, Guid.NewGuid().ToString("N"));

                void OnTreeChanged(object sender, TreeChangedEventArgs<IBaseElement> eventargs)
                {
                    triggered = true;
                }

                testiotcore.Root.TreeChanged += OnTreeChanged;

                triggered = false;
                Assert.Throws<ArgumentNullException>(() => testiotcore.ElementManager.CreateStructureElement(testiotcore.Root, (string)null, raiseTreeChanged:true)); //adding null
                Assert.That(triggered, Is.False);

                triggered = false;
                Assert.Throws(Is.InstanceOf(typeof(IoTCoreException)), () => testiotcore.ElementManager.CreateStructureElement(testiotcore.Root, struct0.Identifier, raiseTreeChanged: true)); //adding same identifier again
                Assert.That(triggered, Is.False);

                triggered = false;
                testiotcore.ElementManager.RemoveElement(testiotcore.Root, struct0, true); //remove element
                Assert.That(triggered);

                triggered = false;
                Assert.Throws(Is.InstanceOf(typeof(IoTCoreException)), () => testiotcore.ElementManager.RemoveElement(testiotcore.Root, struct0, true)); //remove same element again
                Assert.That(triggered, Is.False);

                triggered = false;
                Assert.Throws<ArgumentNullException>(() => testiotcore.ElementManager.RemoveElement(testiotcore.Root, null, true)); //remove null
                Assert.That(triggered, Is.False);

                testiotcore.Root.TreeChanged -= OnTreeChanged;
            }
        }
    }
}
