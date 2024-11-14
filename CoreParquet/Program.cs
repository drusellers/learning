// See https://aka.ms/new-console-template for more information

using Parquet;

var pwd = Environment.CurrentDirectory;
var path = Path.Join(pwd, "AltasParquetDaily-00001.snappy.parquet");
var p = await ParquetReader.ReadTableFromFileAsync(path);
Console.WriteLine(p.Count);
