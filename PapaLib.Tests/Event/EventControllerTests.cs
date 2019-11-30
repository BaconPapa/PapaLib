using System;
using PapaLib.Event;
using Xunit;

namespace PapaLib.Tests.Event
{
    public class EventControllerTests
    {
        private EventController eventController;

        public EventControllerTests()
        {
            eventController = new EventController();
        }

        [Fact]
        public void Add_0Args_Call_Once()
        {
            var count = 0;
            eventController.AddEventListener("event", () => count++);
            eventController.DispatchEvent("event");
            Assert.Equal(1, count);
        }

        [Fact]
        public void Add_0Args_Call_Remove_Listener_Call()
        {
            var count = 0;
            Action callback = () => count++;
            eventController.AddEventListener("event", callback);
            eventController.DispatchEvent("event");
            eventController.RemoveEventListener("event", callback);
            eventController.DispatchEvent("event");
            Assert.Equal(1, count);
        }

        [Fact]
        public void Add_1Args_Call_Once()
        {
            var count = 0;
            eventController.AddEventListener<int>("event", (num1) => count += num1);
            eventController.DispatchEvent("event", 1);
            Assert.Equal(1, count);
        }

        [Fact]
        public void Add_1Args_Call_Remove_Listener_Call()
        {
            var count = 0;
            Action<int> callback = (num1) => count += num1;
            eventController.AddEventListener("event", callback);
            eventController.DispatchEvent("event", 1);
            eventController.RemoveEventListener("event", callback);
            eventController.DispatchEvent("event", 1);
            Assert.Equal(1, count);
        }

        [Fact]
        public void Add_2Args_Call_Once()
        {
            var count = 0;
            eventController.AddEventListener<int, int>("event", (num1, num2) => count += num1 + num2);
            eventController.DispatchEvent("event", 1, 1);
            Assert.Equal(2, count);
        }

        [Fact]
        public void Add_2Args_Call_Remove_Listener_Call()
        {
            var count = 0;
            Action<int, int> callback = (num1, num2) => count += num1 + num2;
            eventController.AddEventListener("event", callback);
            eventController.DispatchEvent("event", 1, 1);
            eventController.RemoveEventListener("event", callback);
            eventController.DispatchEvent("event", 1, 1);
            Assert.Equal(2, count);
        }

        [Fact]
        public void Add_3Args_Call_Once()
        {
            var count = 0;
            eventController.AddEventListener<int, int, int>("event", (num1, num2, num3) => count += num1 + num2 + num3);
            eventController.DispatchEvent("event", 1, 1, 1);
            Assert.Equal(3, count);
        }

        [Fact]
        public void Add_3Args_Call_Remove_Listener_Call()
        {
            var count = 0;
            Action<int, int, int> callback = (num1, num2, num3) => count += num1 + num2 + num3;
            eventController.AddEventListener("event", callback);
            eventController.DispatchEvent("event", 1, 1, 1);
            eventController.RemoveEventListener("event", callback);
            eventController.DispatchEvent("event", 1, 1, 1);
            Assert.Equal(3, count);
        }

        [Fact]
        public void Add_4Args_Call_Once()
        {
            var count = 0;
            eventController.AddEventListener<int, int, int, int>("event", (num1, num2, num3, num4) => count += num1 + num2 + num3 + num4);
            eventController.DispatchEvent("event", 1, 1, 1, 1);
            Assert.Equal(4, count);
        }

        [Fact]
        public void Add_4Args_Call_Remove_Listener_Call()
        {
            var count = 0;
            Action<int, int, int, int> callback = (num1, num2, num3, num4) => count += num1 + num2 + num3 + num4;
            eventController.AddEventListener("event", callback);
            eventController.DispatchEvent("event", 1, 1, 1, 1);
            eventController.RemoveEventListener("event", callback);
            eventController.DispatchEvent("event", 1, 1, 1, 1);
            Assert.Equal(4, count);
        }
    }
}