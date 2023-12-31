﻿using ReactiveUI;
using SchoolProjectA_ClientMVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProjectA_ClientMVVM.ViewModels;
public class GlobalViewModel : ViewModelBase
{
    private ViewModelBase _containedViewModel;
    public Moni MyMoni { get; }
    public GlobalViewModel(Moni myMoni)
    {
        _containedViewModel = new AccountsViewModel(myMoni.MoniId);
        MyMoni = myMoni;
        /* 
         * Debug 
         */
        System.Diagnostics.Debug.WriteLine($"{MyMoni.MoniLogin}:\n\t- Nom: {MyMoni.LastName}\n\t- Prénom: {MyMoni.FirstName}");
    }

    public ViewModelBase ContainedViewModel
    {
        get => _containedViewModel;
        set => this.RaiseAndSetIfChanged(ref _containedViewModel, value);
    }

    public void ShowTransactions()
    {
        ContainedViewModel = new TransactionsViewModel(MyMoni.MoniId);
    }

    public void ShowAccounts()
    {
        ContainedViewModel = new AccountsViewModel(MyMoni.MoniId);
    }

    public void ShowTags()
    {
        ContainedViewModel = new TagsViewModel(MyMoni.MoniId);
    }

}
