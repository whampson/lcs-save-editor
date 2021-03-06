﻿using System;
using System.Windows;
using System.Windows.Controls;
using GTASaveData.LCS;
using LCSSaveEditor.Core;

namespace LCSSaveEditor.GUI.Views
{
    public class TabPageBase<T> : UserControl
        where T : ViewModels.TabPageBase
    {
        public T ViewModel
        {
            get { return (T) DataContext; }
            set { DataContext = value; }
        }

        public TabPageBase()
            : base()
        {
            Loaded += View_Loaded;
        }

        public Editor TheEditor => Editor.TheEditor;
        public LCSSave TheSave => TheEditor.ActiveFile;
        public Settings TheSettings => Settings.TheSettings;
        public Gxt TheText => Gxt.TheText;
        public Carcols TheCarcols => Carcols.TheCarcols;

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            // Changing terminology here to make event flow consistent with view models
            OnInitialize();
        }

        private void ViewModel_ShuttingDown(object sender, EventArgs e)
        {
            OnShutdown();
        }

        private void ViewModel_Loading(object sender, EventArgs e)
        {
            OnLoad();
        }

        private void ViewModel_Unloading(object sender, EventArgs e)
        {
            OnUnload();
            RaiseEvent(new RoutedEventArgs(UnloadedEvent));
        }

        private void ViewModel_Updating(object sender, EventArgs e)
        {
            OnUpdate();
        }

        protected virtual void OnInitialize()
        {
            if (ViewModel != null)
            {
                ViewModel.ShuttingDown += ViewModel_ShuttingDown;
                ViewModel.Loading += ViewModel_Loading;
                ViewModel.Unloading += ViewModel_Unloading;
                ViewModel.Updating += ViewModel_Updating;
            }
        }

        protected virtual void OnShutdown()
        {
            if (ViewModel != null)
            {
                ViewModel.Updating -= ViewModel_Updating;
                ViewModel.Unloading -= ViewModel_Unloading;
                ViewModel.Loading -= ViewModel_Loading;
                ViewModel.ShuttingDown -= ViewModel_ShuttingDown;
                Loaded -= View_Loaded;
            }
        }

        protected virtual void OnLoad()
        { }

        protected virtual void OnUnload()
        { }

        protected virtual void OnUpdate()
        { }
    }
}
