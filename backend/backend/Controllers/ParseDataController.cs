﻿using backend.Data;
using backend.DTOs;
using backend.Interfaces;
using backend.Services.FindPathService;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    /// <summary>
    /// НУЖЕН ТОЛЬКО ДЛЯ ТЕСТОВ ПАРСИНГА
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ParseDataController : ControllerBase
    {
        private readonly IFileParser _fileParser;
        private readonly DataContext dataContext;
        public ParseDataController(IFileParser fileParser, DataContext context)
        {
            _fileParser = fileParser;
            dataContext = context;
        }

        /// <summary>
        /// парсинг файла(нужен для тестов, так то нахуй не сдался)
        /// </summary>
        /// <returns></returns>
        [HttpPost("/parseData")]
        public async Task<ActionResult<List<Point>>> LoadFile([FromBody] ParserDTO parserDTO)
        {
            double[,] parseData = await _fileParser.FileParser(parserDTO);
            double[][] doubles = new double[parseData.GetLength(0)][];

            for (int i = 0; i < parseData.GetLength(0); i++)
            {
                doubles[i] = Enumerable.Range(0, parseData.GetUpperBound(1) + 1)
                 .Select(j => parseData[i, j])
                      .ToArray();
            }

            doubles = doubles
                .AsParallel()
                .AsOrdered()
                .Select(x => x.Select(y => y <= 0 ? 100 : 1 / y)
                .ToArray())
                .ToArray();


            AStar aStar = new AStar(To2D(doubles));
            List<Point> path = aStar.FindPath(new Point() { X = 140, Y = 51 }, new Point() { X = 56, Y = 48});
            return path;
        }

        static T[,] To2D<T>(T[][] source)
        {
            try
            {
                int FirstDim = source.Length;
                int SecondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

                var result = new T[FirstDim, SecondDim];
                for (int i = 0; i < FirstDim; ++i)
                    for (int j = 0; j < SecondDim; ++j)
                        result[i, j] = source[i][j];

                return result;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("The given jagged array is not rectangular.");
            }
        }
    }
}
