namespace TesteRec.Db.Models
{
    public class Menu
    {
        public string Descricao { get; set; }
        public string Imagem { get; set; }
    }
    public static class DbMenu
    {
        public static List<Menu> _menusRelatorios = new List<Menu>()
        {
            new Menu()
            {
                Descricao = "Geral",
                Imagem = "geral.png"
            },
            new Menu()
            {
                Descricao = "Serviço",
                Imagem = "oleo.png"
            },
            new Menu()
            {
                Descricao = "Ganhos",
                Imagem = "bolsadedinheiro.png"
            },
            new Menu()
            {
                Descricao = "Gastos",
                Imagem = "despesa.png"
            },
            new Menu()
            {
                Descricao = "Abastecimento",
                Imagem = "postomenu.png"
            },
        };

        public static List<Menu> _menus = new List<Menu>()
        {
            new Menu()
            {
                Descricao = "Lembrete",
                Imagem = "sino.png"
            },
            //new Menu()
            //{
            //    Descricao = "Checklist",
            //    Imagem = "checklist.png"
            //},
            new Menu()
            {
                Descricao = "Percurso",
                Imagem = "caminho.png"
            },
            new Menu()
            {
                Descricao = "Serviço",
                Imagem = "oleo.png"
            },
            new Menu()
            {
                Descricao = "Ganhos",
                Imagem = "bolsadedinheiro.png"
            },
            new Menu()
            {
                Descricao = "Gastos",
                Imagem = "despesa.png"
            },
            new Menu()
            {
                Descricao = "Abastecimento",
                Imagem = "postomenu.png"
            },
        };
    }
}
