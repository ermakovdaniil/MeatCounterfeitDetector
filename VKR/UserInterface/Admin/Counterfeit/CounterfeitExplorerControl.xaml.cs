﻿using System.Windows.Controls;

using MeatCountefeitDetector.Utils;


namespace MeatCountefeitDetector.UserInterface.Admin.Counterfeit;

public partial class CounterfeitExplorerControl : UserControl
{
    private readonly CounterfeitExplorerControlVM _viewModel;

    public CounterfeitExplorerControl()
    {
        InitializeComponent();
        _viewModel = (CounterfeitExplorerControlVM?)VmLocator.Resolve<CounterfeitExplorerControl>();
        DataContext = _viewModel;
    }
}

