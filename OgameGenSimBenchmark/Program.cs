// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using OgameGenSimBenchmark.Classes;
using OgameGenSimBenchmark.SimulatorPack;

HttpClient client = new HttpClient();

var summary = BenchmarkRunner.Run<BattleBenchmark>();
