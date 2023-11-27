using ClientAPI.DTO.CounterfeitImage;
using ClientAPI.DTO.Result;
using ImageWorker.Enums;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace ImageWorker.ImageAnalyzis;

public interface IImageAnalyzer
{
    public CreateResultDTO RunAnalysis(BitmapSource originalImage, List<GetCounterfeitImageDTO> counterfeitImages, double percentOfSimilarity, Guid userId, Algorithms algorithm, string fileName);
}