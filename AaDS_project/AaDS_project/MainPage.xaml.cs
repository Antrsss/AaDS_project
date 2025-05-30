﻿namespace AaDS_project
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void GoToTheDataStructuresPage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("/DataStructuresPage");
        }

        private async void GoToTheDynamicProgrammingPage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("/DynamicProgrammingPage");
        }

        private async void GoToTaskPage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("/TaskPage");
        }

        private async void GoToTheBinarySearchTreesPage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("/BinarySearchTreesPage");
        }
    }
}
