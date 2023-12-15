using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace SchoolProjectA_ClientMVVM.Views;

public partial class ShowAccountsView : UserControl
{
    int Count { get; set; } = 0;
    public ShowAccountsView()
    {
        InitializeComponent();

        Loaded += (sender, e) =>
        {
            AfficherParents(this);
        };
    }

    private void AfficherParents(Control control)
    {
        if (control?.Parent is Control parentControl)
        {
            System.Diagnostics.Debug.WriteLine("Nom du parent : " + parentControl.GetType().Name + $" {Count}");
            Count++;
            AfficherParents(parentControl); // Appel récursif pour afficher le parent du parent
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("Plus de parent ou le parent n'est pas un Control.");
        }
    }
}