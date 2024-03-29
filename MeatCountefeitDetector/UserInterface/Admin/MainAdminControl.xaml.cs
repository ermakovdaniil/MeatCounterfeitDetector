﻿using System.Windows.Controls;

using MeatCounterfeitDetector.Utils;


namespace MeatCounterfeitDetector.UserInterface.Admin;

public partial class MainAdminControl : UserControl
{
    private readonly MainAdminControlVM _viewModel;

    public MainAdminControl()
    {
        InitializeComponent();
        _viewModel = (MainAdminControlVM?)VmLocator.Resolve<MainAdminControl>();
        DataContext = _viewModel;
    }
}
