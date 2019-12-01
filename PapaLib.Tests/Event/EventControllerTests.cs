using System;
using PapaLib.Event;
using Xunit;

namespace PapaLib.Tests.Event
{
    public class EventControllerTests
    {
        private readonly EventController _eventController;

        public EventControllerTests()
        {
            _eventController = new EventController();
        }

        [Fact]
        public void Add_0Args_Call_Once()
        {
            var count = 0;
            _eventController.AddEventListener("event", () => count++);
            _eventController.DispatchEvent("event");
            Assert.Equal(1, count);
        }

        [Fact]
        public void Add_0Args_Call_Remove_Listener_Call()
        {
            var count = 0;
            void Callback() => count++;
            _eventController.AddEventListener("event", Callback);
            _eventController.DispatchEvent("event");
            _eventController.RemoveEventListener("event", Callback);
            _eventController.DispatchEvent("event");
            Assert.Equal(1, count);
        }

        [Fact]
        public void Add_1Args_Call_Once()
        {
            var count = 0;
            _eventController.AddEventListener<int>("event", (num1) => count += num1);
            _eventController.DispatchEvent("event", 1);
            Assert.Equal(1, count);
        }

        [Fact]
        public void Add_1Args_Call_Remove_Listener_Call()
        {
            var count = 0;
            void Callback(int num1) => count += num1;
            _eventController.AddEventListener("event", (Action<int>) Callback);
            _eventController.DispatchEvent("event", 1);
            _eventController.RemoveEventListener("event", (Action<int>) Callback);
            _eventController.DispatchEvent("event", 1);
            Assert.Equal(1, count);
        }

        [Fact]
        public void Add_2Args_Call_Once()
        {
            var count = 0;
            _eventController.AddEventListener<int, int>("event", (num1, num2) => count += num1 + num2);
            _eventController.DispatchEvent("event", 1, 1);
            Assert.Equal(2, count);
        }

        [Fact]
        public void Add_2Args_Call_Remove_Listener_Call()
        {
            var count = 0;
            void Callback(int num1, int num2) => count += num1 + num2;
            _eventController.AddEventListener("event", (Action<int, int>) Callback);
            _eventController.DispatchEvent("event", 1, 1);
            _eventController.RemoveEventListener("event", (Action<int, int>) Callback);
            _eventController.DispatchEvent("event", 1, 1);
            Assert.Equal(2, count);
        }

        [Fact]
        public void Add_3Args_Call_Once()
        {
            var count = 0;
            _eventController.AddEventListener<int, int, int>("event", (num1, num2, num3) => count += num1 + num2 + num3);
            _eventController.DispatchEvent("event", 1, 1, 1);
            Assert.Equal(3, count);
        }

        [Fact]
        public void Add_3Args_Call_Remove_Listener_Call()
        {
            var count = 0;
            void Callback(int num1, int num2, int num3) => count += num1 + num2 + num3;
            _eventController.AddEventListener("event", (Action<int, int, int>) Callback);
            _eventController.DispatchEvent("event", 1, 1, 1);
            _eventController.RemoveEventListener("event", (Action<int, int, int>) Callback);
            _eventController.DispatchEvent("event", 1, 1, 1);
            Assert.Equal(3, count);
        }

        [Fact]
        public void Add_4Args_Call_Once()
        {
            var count = 0;
            _eventController.AddEventListener<int, int, int, int>("event", (num1, num2, num3, num4) => count += num1 + num2 + num3 + num4);
            _eventController.DispatchEvent("event", 1, 1, 1, 1);
            Assert.Equal(4, count);
        }

        [Fact]
        public void Add_4Args_Call_Remove_Listener_Call()
        {
            var count = 0;
            void Callback(int num1, int num2, int num3, int num4) => count += num1 + num2 + num3 + num4;
            _eventController.AddEventListener("event", (Action<int, int, int, int>) Callback);
            _eventController.DispatchEvent("event", 1, 1, 1, 1);
            _eventController.RemoveEventListener("event", (Action<int, int, int, int>) Callback);
            _eventController.DispatchEvent("event", 1, 1, 1, 1);
            Assert.Equal(4, count);
        }
    }
}