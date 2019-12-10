namespace Servico
{
    public class Conexao
    {
        public readonly string stringConexao;

        public Conexao()
        {
            stringConexao = "Server=ecommercedb.sifo.tech;Port=3306;User ID=admin;Password=admin;Database=ecommerce;AllowPublicKeyRetrieval=True";
        }
    }
}
