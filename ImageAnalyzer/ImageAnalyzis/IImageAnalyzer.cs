﻿using DataAccess.Models;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace ImageWorker.ImageAnalyzis;

public interface IImageAnalyzer
{
    public Result RunAnalysis(string pathToOrig, List<CounterfeitPath> counterfeitPaths, double percentOfSimilarity, User user);
}