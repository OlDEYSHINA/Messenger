namespace Client.Services
{
    using System.Windows;

    internal class ChatMenuService
    {
        #region Methods

        public void Exit()
        {
            Application.Current.Shutdown();
        }

        public void About()
        {
            MessageBox.Show("Программа для обмена сообщениями."
                            + "\nСделано Подкорытовым Степаном. \nПо всем вопросам обращаться к Васюткину Кириллу.", "About");
        }

        #endregion
    }
}
