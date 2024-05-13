namespace ifm.IoTCore.UnitTests;

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ElementManager.Contracts.Elements;
using Factory;
using NUnit.Framework;

[TestFixture]
public class ConcurrencyTests
{
    [Test]
    public void MultiThreadedAddElement_AllElementsExist_Success()
    {
        using var ioTCore = IoTCoreFactory.Create("id0");

        // Arrange
        var stopWatch = new Stopwatch();

        try
        {

            stopWatch.Start();
            var tasks = new List<Task>();
            for (var i = 0; i < 100; i++)
            {
                var i1 = i;
                var task = Task.Run(() =>
                {
                    for (var j = 0; j < 100; j++)
                    {
                        try
                        {
                            ioTCore.ElementManager.CreateStructureElement(ioTCore.Root, $"structure{i1}-{j}");
                        }
                        catch 
                        {
                            // Ignore
                        }
                        //var dataElement = new DataElement($"data{i1}-{j}", null, null, null);
                        //structureElement.AddElement(dataElement);
                        //var serviceElement = new ServiceElement($"service{i1}-{j}", null, null, null);
                        //structureElement.AddElement(serviceElement);
                        //var eventElement = new EventElement($"event{i1}-{j}", null, null, null);
                        //structureElement.AddElement(eventElement);
                    }
                });
                tasks.Add(task);
            }

                
            Task.WaitAll(tasks.ToArray());
            TestContext.WriteLine($"Elements created takes {stopWatch.ElapsedMilliseconds} ms");

            // Act
            stopWatch.Restart();
            var elements = new List<IBaseElement>();
            for (var i = 0; i < 100; i++)
            {
                for (var j = 0; j < 100; j++)
                {
                    var element = ioTCore.Root.GetElementByIdentifier($"structure{i}-{j}");
                    if (element != null) elements.Add(element);
                    //element = _ioTCore.Root.GetElementByIdentifier($"data{i}-{j}");
                    //if (element != null) elements.Add(element);
                    //element = _ioTCore.Root.GetElementByIdentifier($"service{i}-{j}");
                    //if (element != null) elements.Add(element);
                    //element = _ioTCore.Root.GetElementByIdentifier($"event{i}-{j}");
                    //if (element != null) elements.Add(element);
                }
            }

            TestContext.WriteLine($"Elements collected by identifier takes {stopWatch.ElapsedMilliseconds} ms");

            // Assert
            Assert.That(elements.Count == 100 * 100);
            //Assert.That(elements.Count == 100 * 100 * 4);

            // Act
            elements.Clear();
            stopWatch.Restart();
            for (var i = 0; i < 100; i++)
            {
                for (var j = 0; j < 100; j++)
                {
                    var element = ioTCore.ElementManager.GetElementByAddress($"id0/structure{i}-{j}");
                    if (element != null) elements.Add(element);
                    //element = _ioTCore.GetElementByIdentifier($"data{i}-{j}");
                    //if (element != null) elements.Add(element);
                    //element = _ioTCore.GetElementByIdentifier($"service{i}-{j}");
                    //if (element != null) elements.Add(element);
                    //element = _ioTCore.GetElementByIdentifier($"event{i}-{j}");
                    //if (element != null) elements.Add(element);
                }
            }

            TestContext.WriteLine($"Elements collected by address takes {stopWatch.ElapsedMilliseconds} ms");

            Assert.That(elements.Count == 100 * 100);

            // Act
            elements.Clear();
            stopWatch.Restart();
            for (var i = 0; i < 100; i++)
            {
                for (var j = 0; j < 100; j++)
                {
                    var element = ioTCore.ElementManager.GetElementByAddress($"/structure{i}-{j}");
                    if (element != null) elements.Add(element);
                    //element = _ioTCore.GetElementByIdentifier($"data{i}-{j}");
                    //if (element != null) elements.Add(element);
                    //element = _ioTCore.GetElementByIdentifier($"service{i}-{j}");
                    //if (element != null) elements.Add(element);
                    //element = _ioTCore.GetElementByIdentifier($"event{i}-{j}");
                    //if (element != null) elements.Add(element);
                }
            }

            TestContext.WriteLine(
                $"Elements collected by address from element manager takes {stopWatch.ElapsedMilliseconds} ms");

            Assert.That(elements.Count == 100 * 100);
        }
        finally
        {
            stopWatch.Stop();
            ioTCore.Dispose();
        }
    }
}