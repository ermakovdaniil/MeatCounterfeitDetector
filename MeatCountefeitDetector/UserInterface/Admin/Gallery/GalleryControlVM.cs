﻿using System.Linq;
using System.Windows;
using MeatCounterfeitDetector.UserInterface.Admin.Abstract;
using MeatCounterfeitDetector.Utils;
using MeatCounterfeitDetector.Utils.Dialog;
using MeatCounterfeitDetector.Utils.MessageBoxService;
using ClientAPI;
using ClientAPI.DTO.CounterfeitImage;
using Mapster;
using System.Collections.ObjectModel;
using MeatCounterfeitDetector.UserInterface.EntityVM;
using MeatCounterfeitDetector.Utils.EventAggregator;
using System;
using System.IO;

namespace MeatCounterfeitDetector.UserInterface.Admin.Gallery;

public class GalleryControlVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public GalleryControlVM(CounterfeitImageClient counterfeitImageClient,
                            DialogService dialogService,
                            IMessageBoxService messageBoxService,
                            IEventAggregator eventAggregator)
    {
        _counterfeitImageClient = counterfeitImageClient;
        _messageBoxService = messageBoxService;
        _dialogService = dialogService;

        _eventAggregator = eventAggregator;
        _eventAggregator.Subscribe<Event>(CollectionChanged);

        _counterfeitImageClient.CounterfeitImageGetAsync()
                               .ContinueWith(c => { CounterfeitImageVMs = c.Result.ToList().Adapt<ObservableCollection<CounterfeitImageVM>>(); });
    }

    #endregion

    private void CollectionChanged(Event data)
    {
        _counterfeitImageClient.CounterfeitImageGetAsync()
                               .ContinueWith(c => { CounterfeitImageVMs = c.Result.ToList().Adapt<ObservableCollection<CounterfeitImageVM>>(); });

        OnPropertyChanged(nameof(CounterfeitImageVMs));
    }

    #endregion


    #region Properties

    private readonly DialogService _dialogService;
    private readonly CounterfeitImageClient _counterfeitImageClient;
    private readonly IEventAggregator _eventAggregator;
    private readonly IMessageBoxService _messageBoxService;

    public ObservableCollection<CounterfeitImageVM> CounterfeitImageVMs { get; set; }
    public CounterfeitImageVM SelectedCounterfeitImage { get; set; }

    #endregion


    #region Commands

    private RelayCommand _addCounterfeitImage;
    public RelayCommand AddCounterfeitImage
    {
        get
        {
            return _addCounterfeitImage ??= new RelayCommand(async o =>
            {
                //Application.Current.Dispatcher.Invoke(async () =>
                //{
                var result = (await _dialogService.ShowDialog<GalleryEditControl>(new WindowParameters
                {
                    Height = 470,
                    Width = 450,
                    Title = "Добавление изображения фальсификата",
                },
                data: new CounterfeitImageVM())) as CounterfeitImageVM;

                if (result is null)
                {
                    return;
                }

                if (!CounterfeitImageVMs.Any(rec => rec.CounterfeitId == result.CounterfeitId && rec.ImagePath == result.ImagePath))
                {
                    if (File.Exists(result.ImagePath))
                    {
                        var newImagePath = @"..\..\..\resources\counterfeits\" + Path.GetFileName(result.ImagePath);
                        File.Copy(result.ImagePath, newImagePath);
                        result.ImagePath = newImagePath;

                        var addingCounterfeitImageCreateDTO = result.Adapt<CreateCounterfeitImageDTO>();
                        var id = await _counterfeitImageClient.CounterfeitImagePostAsync(addingCounterfeitImageCreateDTO);
                        result.Id = id;
                        CounterfeitImageVMs.Add(result);
                        _messageBoxService.ShowMessage($"Запись успешно добавлена!", "Готово!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        _messageBoxService.ShowMessage($"Невозможно добавить такой файл!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    _messageBoxService.ShowMessage($"Такая запись уже существует!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                //});
            });
        }
    }

    private RelayCommand _editCounterfeitImage;
    public RelayCommand EditCounterfeitImage
    {
        get
        {
            return _editCounterfeitImage ??= new RelayCommand(async o =>
            {
                //Application.Current.Dispatcher.Invoke(async () =>
                //{
                var tempCounterfeitImage = (CounterfeitImageVM)SelectedCounterfeitImage.Clone();

                var path = SelectedCounterfeitImage.ImagePath;

                var result = (await _dialogService.ShowDialog<GalleryEditControl>(new WindowParameters
                {
                    Height = 470,
                    Width = 450,
                    Title = "Редактирование изображения фальсификата",
                },
                data: tempCounterfeitImage)) as CounterfeitImageVM;

                if (result is null)
                {
                    return;
                }

                if (!CounterfeitImageVMs.Any(rec => rec.CounterfeitId == result.CounterfeitId && rec.ImagePath == result.ImagePath))
                {
                    if (File.Exists(result.ImagePath))
                    {
                        var newImagePath = @"..\..\..\resources\counterfeits\" + Path.GetFileName(result.ImagePath);
                        File.Copy(result.ImagePath, newImagePath, true);
                        result.ImagePath = newImagePath;

                        var editingCounterfeitImageUpdateDTO = result.Adapt<UpdateCounterfeitImageDTO>();
                        await _counterfeitImageClient.CounterfeitImagePutAsync(editingCounterfeitImageUpdateDTO);
                        result.Adapt(CounterfeitImageVMs.FirstOrDefault(x => x.Id == result.Id));
                        _messageBoxService.ShowMessage($"Запись успешно обновлена!", "Готово!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        _messageBoxService.ShowMessage($"Невозможно добавить такой файл!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    } 
                }
                else
                {
                    _messageBoxService.ShowMessage($"Такая запись уже существует!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                //});

            }, _ => SelectedCounterfeitImage is not null);
        }
    }

    private RelayCommand _deleteCounterfeitImage;
    public RelayCommand DeleteCounterfeitImage
    {
        get
        {
            return _deleteCounterfeitImage ??= new RelayCommand(async o =>
            {
                if (_messageBoxService.ShowMessage("Вы действительно хотите удалить изображение?", "Удаление изображения", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    //string pathToBase = Directory.GetCurrentDirectory();
                    //string combinedPath = Path.Combine(pathToBase, SelectedCounterfeitImage.ImagePath);
                    //File.Delete(combinedPath);

                    //_context.CounterfeitImages.Remove(SelectedCounterfeitImage);
                    //_context.SaveChanges();

                    //Application.Current.Dispatcher.Invoke(async () =>
                    //{
                    File.Delete(SelectedCounterfeitImage.ImagePath);
                    await _counterfeitImageClient.CounterfeitImageDeleteAsync(SelectedCounterfeitImage.Id);
                    CounterfeitImageVMs.Remove(SelectedCounterfeitImage);
                    _messageBoxService.ShowMessage($"Запись успешно удалена!", "Готово!", MessageBoxButton.OK, MessageBoxImage.Information);
                    //});
                }
            }, c => SelectedCounterfeitImage is not null);
        }
    }

    #endregion
}