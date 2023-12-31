﻿using Microsoft.Maui.Controls;

namespace MauiApp2;

public partial class MainPage : ContentPage
{
	int count = 0;
	double h = -40;
	List<Grid> grids = new List<Grid>();
    private bool isAnimatingCharacter = false;
	public MainPage()
	{
		InitializeComponent();
        this.Loaded += MainPage_Loaded;
	}

    private void MainPage_Loaded(object sender, EventArgs e)
    {
		grids = parentGrid.Children.OfType<Grid>().ToList();	
    }
	private async void animateIn(double tr, Grid g, uint t) {
		await g.TranslateTo(0, tr, t, Easing.SinInOut);
		
	}
    private async void animateCharacter() { 
        if (!isAnimatingCharacter)
        {
            isAnimatingCharacter = true;
            imgCharacter.TranslateTo(0, -100, 500, Easing.SinInOut);
            imgCharacter.RotateTo(30, 500, Easing.SinInOut);
            await imgCharacter.ScaleTo(5.5, 500, Easing.SinInOut);
            imgCharacter.TranslateTo(0, -65, 400, Easing.SinInOut);
            imgCharacter.RotateTo(0, 399, Easing.SinInOut);
            await imgCharacter.ScaleTo(5, 400, Easing.SinInOut);
            imgCharacter.Rotation = 0;
            isAnimatingCharacter = false;
        }
    }
   

    private void txtEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        animateCharacter();
		int weight = 0;
		int.TryParse(txtWeight.Text, out weight);
        int goal = 0;
        int.TryParse(txtGoal.Text, out goal);
        int fat = 0;
        int.TryParse(txtCurrent.Text, out fat);
		Double mybase = (Double)weight - ((Double)fat / 100) * weight;
		var gg = mybase / (1 -(double)goal / 100);
        if (gg > 999) {
			return;
		}
		txtInfo.Text = "To reach your desired body fat percentage you will need to get to around " + Convert.ToInt32(gg).ToString() +  "lbs. You need to lose " + Convert.ToInt32((weight - gg)).ToString() + "lbs to reach that goal. This is just an approximation off the given body fat and weight, depending on the muscle you add, water etc it could be slightly different.";
		var digs = GetDigits(Convert.ToInt32(gg)).ToList();
		var diff = grids.Count() - digs.Count();

        if (diff > 0) {
			var id = 0;
			while (diff > 0) {
                digs.Insert(id, 0);
				diff--;
				id++;
            }
			
		}
		var st = 0;
		uint ttt = 1000;
		foreach (var ds in digs) {
			var dist = ds * h;
			animateIn(dist, grids[st], ttt);
			st++;
			ttt = ttt + 200;
		}
		
    }
    public static IEnumerable<int> GetDigits(int source)
    {
        int individualFactor = 0;
        int tennerFactor = Convert.ToInt32(Math.Pow(10, source.ToString().Length));
        do
        {
            source -= tennerFactor * individualFactor;
            tennerFactor /= 10;
            individualFactor = source / tennerFactor;

            yield return individualFactor;
        } while (tennerFactor > 1);
    }
    private async void ClickGestureRecognizer_Clicked(object sender, EventArgs e)
    {
        Uri uri = new Uri("https://pishah.com");
        await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
    }

    private void PointerGestureRecognizer_PointerEntered(object sender, PointerEventArgs e)
    {
        txtTrademark.TextColor = Colors.White;
        txtTrademark.FontAttributes = FontAttributes.Italic;
    }

    private void PointerGestureRecognizer_PointerExited(object sender, PointerEventArgs e)
    {

        txtTrademark.TextColor = Colors.White;
        txtTrademark.FontAttributes = FontAttributes.None;
    }
}

