// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// This application uses the Azure IoT Hub device SDK for .NET
// For samples see: https://github.com/Azure/azure-iot-sdk-csharp/tree/main/iothub/device/samples

using Microsoft.Azure.Devices.Client;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;

using System.IO;
using CsvHelper;

namespace SimulatedDevice
{
    /// <summary>
    /// This sample illustrates the very basics of a device app sending telemetry. For a more comprehensive device app sample, please see
    /// <see href="https://github.com/Azure-Samples/azure-iot-samples-csharp/tree/main/iot-hub/Samples/device/DeviceReconnectionSample"/>.
    /// </summary>
    internal class Program
    {
        private static DeviceClient s_deviceClient;
        private static readonly TransportType s_transportType = TransportType.Mqtt;

        // The device connection string to authenticate the device with your IoT hub.
        // Using the Azure CLI:
        // az iot hub device-identity show-connection-string --hub-name {YourIoTHubName} --device-id MyDotnetDevice --output table
        private static string s_connectionString = "{Your device connection string here}";

        private static async Task Main(string[] args)
        {
            Console.WriteLine("IoT Hub Quickstarts #1 - Simulated device.");

            // This sample accepts the device connection string as a parameter, if present
            ValidateConnectionString(args);

            // Connect to the IoT hub using the MQTT protocol
            s_deviceClient = DeviceClient.CreateFromConnectionString(s_connectionString, s_transportType);

            // Set up a condition to quit the sample
            Console.WriteLine("Press control-C to exit.");
            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                cts.Cancel();
                Console.WriteLine("Exiting...");
            };

            // Run the telemetry loop
            await SendDeviceToCloudMessagesAsync(cts.Token);

            // SendDeviceToCloudMessagesAsync is designed to run until cancellation has been explicitly requested by Console.CancelKeyPress.
            // As a result, by the time the control reaches the call to close the device client, the cancellation token source would
            // have already had cancellation requested.
            // Hence, if you want to pass a cancellation token to any subsequent calls, a new token needs to be generated.
            // For device client APIs, you can also call them without a cancellation token, which will set a default
            // cancellation timeout of 4 minutes: https://github.com/Azure/azure-iot-sdk-csharp/blob/64f6e9f24371bc40ab3ec7a8b8accbfb537f0fe1/iothub/device/src/InternalClient.cs#L1922
            await s_deviceClient.CloseAsync();

            s_deviceClient.Dispose();
            Console.WriteLine("Device simulator finished.");
        }

        private static void ValidateConnectionString(string[] args)
        {
            if (args.Any())
            {
                try
                {
                    var cs = IotHubConnectionStringBuilder.Create(args[0]);
                    s_connectionString = cs.ToString();
                }
                catch (Exception)
                {
                    Console.WriteLine($"Error: Unrecognizable parameter '{args[0]}' as connection string.");
                    Environment.Exit(1);
                }
            }
            else
            {
                try
                {
                    _ = IotHubConnectionStringBuilder.Create(s_connectionString);
                }
                catch (Exception)
                {
                    Console.WriteLine("This sample needs a device connection string to run. Program.cs can be edited to specify it, or it can be included on the command-line as the only parameter.");
                    Environment.Exit(1);
                }
            }
        }

        // Async method to send simulated telemetry
        private static async Task SendDeviceToCloudMessagesAsync(CancellationToken ct)
        {
            var records = new List<SensorData>();

            // Load CSV files
            using (var reader = new StreamReader("./data/farm1.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = new SensorData
                    {
                        datesql = csv.GetField("datesql"),
                        Animal_ID = csv.GetField<int>("Animal_ID"),
                        Group_ID = csv.GetField<int>("Group_ID"),
                        Lactation_Num = csv.GetField<int>("Lactation_Num"),
                        DIM = csv.GetField<int>("DIM"),
                        AnimalStatus = csv.GetField<string>("AnimalStatus"),
                        Gynecology_Status = csv.GetField<string>("Gynecology_Status"),
                        Yield = csv.GetField<float>("Yield(gr)"),
                        ProdRate = csv.GetField<float>("ProdRate(gr/hr)"),
                        Fat = csv.GetField<float>("Fat(%)"),
                        Avg_Fat = csv.GetField<float>("Avg_Fat(%)"),
                        Protein = csv.GetField<float>("Protein(%)"),
                        Avg_Protein = csv.GetField<float>("Avg_Protein(%)"),
                        Lactose = csv.GetField<float>("Lactose"),
                        Avg_Lactose = csv.GetField<float>("Avg_Lactose(%)"),
                        Conductivity = csv.GetField<float>("Conductivity"),
                        Avg_Conductivity = csv.GetField<float>("Avg_Conductivity"),
                        Milking_Time = csv.GetField<float>("Milking_Time(seconds)"),
                        Avg_Milking_Time = csv.GetField<float>("Avg_Milking_Time(seconds)"),
                        Activity = csv.GetField<float>("Activity(steps/hr)"),
                        ActivityDeviation = csv.GetField<float>("ActivityDeviation(%)"),
                        RestBout = csv.GetField<float>("RestBout(#)"),
                        RestPerBout = csv.GetField<float>("RestPerBout(min)"),
                        RestRatio = csv.GetField<float>("RestRatio(%)"),
                        RestRestlessness = csv.GetField<float>("RestRestlessness"),
                        RestTime = csv.GetField<float>("RestTime(min)"),
                        Weight = csv.GetField<float>("Weight(gr)")
                    };
                    // Console.WriteLine(record.Animal_ID);
                    records.Add(record);
                }
            }
            Console.WriteLine($"{DateTime.Now} > Loaded Simulator Data");

            var index = 0;
            while (!ct.IsCancellationRequested)
            {
                // Create JSON message
                string messageBody = JsonSerializer.Serialize(
                    new
                    {
                        datesql = records[index].datesql,
                        Animal_ID = records[index].Animal_ID,
                        Group_ID = records[index].Group_ID,
                        Lactation_Num = records[index].Lactation_Num,
                        DIM = records[index].DIM,
                        AnimalStatus = records[index].AnimalStatus,
                        Gynecology_Status = records[index].Gynecology_Status,
                        Yield = records[index].Yield,
                        ProdRate = records[index].ProdRate,
                        Fat = records[index].Fat,
                        Avg_Fat = records[index].Avg_Fat,
                        Protein = records[index].Protein,
                        Avg_Protein = records[index].Avg_Protein,
                        Lactose = records[index].Lactose,
                        Avg_Lactose = records[index].Avg_Lactose,
                        Conductivity = records[index].Conductivity,
                        Avg_Conductivity = records[index].Avg_Conductivity,
                        Milking_Time = records[index].Milking_Time,
                        Avg_Milking_Time = records[index].Avg_Milking_Time,
                        Activity = records[index].Activity,
                        ActivityDeviation = records[index].ActivityDeviation,
                        RestBout = records[index].RestBout,
                        RestPerBout = records[index].RestPerBout,
                        RestRatio = records[index].RestRatio,
                        RestRestlessness = records[index].RestRestlessness,
                        RestTime = records[index].RestTime,
                        Weight = records[index].Weight,
                    });
                using var message = new Message(Encoding.ASCII.GetBytes(messageBody))
                {
                    ContentType = "application/json",
                    ContentEncoding = "utf-8",
                };

                // Add a custom application property to the message.
                // An IoT hub can filter on these properties without access to the message body.
                // message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");

                // Send the telemetry message
                await s_deviceClient.SendEventAsync(message);
                Console.WriteLine($"{DateTime.Now} > Sending message: {messageBody}");

                index = index + 1;

                await Task.Delay(3000);
            }
        }
    }
}
public class SensorData
{
    public string datesql { get; set; }
    public int Animal_ID { get; set; }
    public int Group_ID { get; set; }
    public int Lactation_Num { get; set; }
    public int DIM { get; set; }
    public string AnimalStatus { get; set; }
    public string Gynecology_Status { get; set; }
    public float Yield { get; set; } // Yield(gr)
    public float ProdRate { get; set; } // ProdRate(gr/hr)
    public float Fat { get; set; } // Fat(%)
    public float Avg_Fat { get; set; } // Avg_Fat(%)
    public float Protein { get; set; } // Protein(%)
    public float Avg_Protein { get; set; } // Avg_Protein(%)
    public float Lactose { get; set; } // Lactose
    public float Avg_Lactose { get; set; } // Avg_Lactose(%)
    public float Conductivity { get; set; } // Conductivity
    public float Avg_Conductivity { get; set; } // Avg_Conductivity
    public float Milking_Time { get; set; } // Milking_Time(seconds)
    public float Avg_Milking_Time { get; set; } // Avg_Milking_Time(seconds)
    public float Activity { get; set; } // Activity(steps/hr)
    public float ActivityDeviation { get; set; } // ActivityDeviation(%)
    public float RestBout { get; set; } // RestBout(#)
    public float RestPerBout { get; set; } // RestPerBout(min)
    public float RestRatio { get; set; } // RestRatio(%)
    public float RestRestlessness { get; set; } // RestRestlessness
    public float RestTime { get; set; } // RestTime(min)
    public float Weight { get; set; } // Weight(gr)
}
