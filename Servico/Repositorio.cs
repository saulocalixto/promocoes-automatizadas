using MySql.Data.MySqlClient;

namespace Servico
{
    public class Repositorio : Conexao
    {
        public string Get(int mes)
        {
            string id = string.Empty;

            using (var conn = new MySqlConnection(stringConexao))
            {
                conn.Open();

                using (var cmd = new MySqlCommand(Consulta(mes), conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id = reader.GetString(1);
                        }
                    }
                }
            }

            return id;
        }

        public string Consulta(int mes)
        {
            return $@"select count(product_id) quantidade, product_id 
	                    from olist_order_items_dataset inner join 
		                    olist_orders_dataset on olist_order_items_dataset.order_id = olist_orders_dataset.order_id 
		                    WHERE month(olist_orders_dataset.order_purchase_timestamp) = '{mes}'
		                    group by product_id order  by quantidade desc";
        }
    }
}
