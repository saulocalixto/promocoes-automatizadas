﻿using MySql.Data.MySqlClient;
using System;

namespace Servico
{
    public class RepositorioPromocao : Conexao
    {
        public string GetProdutoMaisVendido(int mes)
        {
            string id = string.Empty;

            using (var conn = new MySqlConnection(stringConexao))
            {
                conn.Open();

                using (var cmd = new MySqlCommand(ConsultaProdutoMaisVendidoPorMes(mes), conn))
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

        /// <summary>
        /// Calcula um coeficiente que define o quão provável uma promoção de um determinado produto deveria ser feita
        /// baseado na média de vendas daquele produto. Quanto maior o coeficiente, maior a recomendação de se fazee
        /// a promoção.
        /// </summary>
        /// <param name="produto">ID do Produto</param>
        /// <param name="mes">Mês desejado (entre 1 e 12)</param>
        /// <returns>Coeficiente com valor entre 0 e 1</returns>
        public float GetSugestaoDePromocao(string produto, int mes)
        {
            int[] quantidadeVendas = new int[12];
            int totalDeVendas = 0;

            using (var conn = new MySqlConnection(stringConexao))
            {
                conn.Open();

                for (int i = 0; i < 12; i++)
                {
                    using (var cmd = new MySqlCommand(ConsultaSugestaoDePromocao(produto, i), conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                quantidadeVendas[i] = reader.GetInt32(0);
                                totalDeVendas += quantidadeVendas[i];
                            }
                        }
                    }
                }
            }

            if (totalDeVendas == 0)
                return -1;

            float coeficienteDeSugestao;
            try
            {
                float media = totalDeVendas / 12f;
                coeficienteDeSugestao = (float)Math.Round(quantidadeVendas[mes - 1] / media, 2);
                if (coeficienteDeSugestao > 1)
                    coeficienteDeSugestao = 1;
            }
            catch
            {
                return -1;
            }

            return 1 - coeficienteDeSugestao;
        }

        /// <summary>
        /// Calcula um coeficiente que define o quão provável uma promoção de um determinado produto deveria ser feita
        /// baseado na média de vendas daquele produto. Quanto maior o coeficiente, maior a recomendação de se fazee
        /// a promoção.
        /// </summary>
        /// <param name="produto">ID do Produto</param>
        /// <param name="mes">Mês desejado (entre 1 e 12)</param>
        /// <param name="dia">Dia desejado (entre 1 e 31, a depender do mês)</param>
        /// <returns>Coeficiente com valor entre 0 e 1</returns>
        public float GetSugestaoDePromocao(string produto, int mes, int dia)
        {
            int totalDeVendas = 0;

            var diasDoMes = DateTime.DaysInMonth(2019, mes);
            int[] quantidadeVendas = new int[diasDoMes];

            using (var conn = new MySqlConnection(stringConexao))
            {
                conn.Open();

                for (int i = 0; i < diasDoMes; i++)
                {
                    using (var cmd = new MySqlCommand(ConsultaSugestaoDePromocao(produto, mes, i), conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                quantidadeVendas[i] = reader.GetInt32(0);
                                totalDeVendas += quantidadeVendas[i];
                            }
                        }
                    }
                }
            }

            if (totalDeVendas == 0)
                return -1;

            float coeficienteDeSugestao;
            try
            {
                float media = (float)totalDeVendas / diasDoMes;
                coeficienteDeSugestao = (float)Math.Round(quantidadeVendas[dia - 1] / media, 2);
                if (coeficienteDeSugestao > 1)
                    coeficienteDeSugestao = 1;
            }
            catch
            {
                return -1;
            }


            return 1 - coeficienteDeSugestao;
        }

        private string ConsultaSugestaoDePromocao(string produto, int mes)
        {
            return $@"SELECT COUNT(product_id) quantidade, product_id 
	                    FROM olist_order_items_dataset 
                        INNER JOIN olist_orders_dataset ON olist_order_items_dataset.order_id = olist_orders_dataset.order_id 
		                WHERE product_id = '{produto}' AND MONTH(olist_orders_dataset.order_purchase_timestamp) = '{mes}' ";
        }

        private string ConsultaSugestaoDePromocao(string produto, int? mes, int? dia)
        {
            var consultaMes = string.Empty;
            var consultaDia = string.Empty;

            if (mes != null)
            {
                consultaMes = $"AND MONTH(olist_orders_dataset.order_purchase_timestamp) = '{mes}'";
            }

            if (dia != null)
            {
                consultaDia = $"AND DAY(olist_orders_dataset.order_purchase_timestamp) = '{dia}'";
            }

            return $@"SELECT COUNT(product_id) quantidade, product_id 
	                    FROM olist_order_items_dataset 
                        INNER JOIN olist_orders_dataset ON olist_order_items_dataset.order_id = olist_orders_dataset.order_id 
		                WHERE product_id = '{produto}' {consultaMes} {consultaDia} ";
        }

        private string ConsultaProdutoMaisVendidoPorMes(int mes)
        {
            return $@"select count(product_id) quantidade, product_id 
	                    from olist_order_items_dataset inner join 
		                    olist_orders_dataset on olist_order_items_dataset.order_id = olist_orders_dataset.order_id 
		                    WHERE month(olist_orders_dataset.order_purchase_timestamp) = '{mes}'
		                    group by product_id order  by quantidade desc";
        }
    }
}
