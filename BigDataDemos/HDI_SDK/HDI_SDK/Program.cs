namespace HDI_SDK
{
    using System;
    using System.Security;
    using Microsoft.Azure;
    using Microsoft.Azure.Common.Authentication;
    using Microsoft.Azure.Common.Authentication.Factories;
    using Microsoft.Azure.Common.Authentication.Models;
    using Microsoft.Azure.Management.HDInsight;
    using Microsoft.Azure.Management.HDInsight.Models;
    using Microsoft.Azure.Management.Resources;

    public class Program
    {
        private static HDInsightManagementClient _hdiManagementClient;

        private static Guid SubscriptionId = new Guid("d77fed05-748d-4c3c-97af-00e6cdd6c46e");
        private const string ExistingResourceGroupName = "HadoopDemo";
        private const string ExistingStorageName = "hdidemoapril.blob.core.windows.net";
        private const string ExistingStorageKey = "1/44YO16M+XVRWoOd/dr8C6aE6feNyzSx1Et7Y7QorrXy7gCxEdWLQlHO1KmNJFSj1o/u1C1xHec0gnxlmhiPA==";
        private const string ExistingBlobContainer = "fmartinezhdidemoaprilsdk";
        private const string NewClusterName = "fmartinezhdidemoaprilsdk";
        private const int NewClusterNumNodes = 1;
        private const string NewClusterLocation = "North Europe";
        private const OSType NewClusterOsType = OSType.Windows;
        private const string NewClusterType = "Hadoop";
        private const string NewClusterVersion = "3.2";
        private const string NewClusterUsername = "fmartinez";
        private const string NewClusterPassword = "Melocotonazo.77";

        static void Main(string[] args)
        {
            System.Console.WriteLine("Comenzando a crear cluster usando .NET SDK");

            Console.Write("Username: ");
            var username = Console.ReadLine();

            Console.Write("Password: ");
            var password = getPassword();

            var tokenCreds = GetTokenCloudCredentials();
            var subCloudCredentials = GetSubscriptionCloudCredentials(tokenCreds, SubscriptionId);

            var existingMetastore = new Metastore("hdimadridsql.database.windows.net", "hdimadridsql", "fmartinez", "Melocotonazo.77");

            var resourceManagementClient = new ResourceManagementClient(subCloudCredentials);
            resourceManagementClient.Providers.Register("Microsoft.HDInsight");

            _hdiManagementClient = new HDInsightManagementClient(subCloudCredentials);

            var parameters = new ClusterCreateParameters
            {
                ClusterSizeInNodes = NewClusterNumNodes,
                UserName = NewClusterUsername,
                Password = NewClusterPassword,
                Location = NewClusterLocation,
                DefaultStorageAccountName = ExistingStorageName,
                DefaultStorageAccountKey = ExistingStorageKey,
                DefaultStorageContainer = ExistingBlobContainer,
                ClusterType = NewClusterType,
                OSType = NewClusterOsType,
                HiveMetastore = existingMetastore
            };

            _hdiManagementClient.Clusters.Create(ExistingResourceGroupName, NewClusterName, parameters);

            System.Console.WriteLine("El cluster ha sido creado");
            System.Console.ReadLine();
        }

        private static SecureString getPassword()
        {
            string pass = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            while (key.Key != ConsoleKey.Enter);

            SecureString secureStr = new SecureString();
            for (int i = 0; i < pass.Length; i++)
            {
                secureStr.AppendChar(pass[i]);
            }
            secureStr.MakeReadOnly();

            return secureStr;
        }

        public static TokenCloudCredentials GetTokenCloudCredentials(string username = null, SecureString password = null)
        {
            var authFactory = new AuthenticationFactory();

            var account = new AzureAccount { Type = AzureAccount.AccountType.User };

            if (username != null && password != null)
                account.Id = username;

            var env = AzureEnvironment.PublicEnvironments[EnvironmentName.AzureCloud];

            ShowDialog dialog = username != null & password != null ? ShowDialog.Never : ShowDialog.Always;

            var accessToken =
                authFactory.Authenticate(account, env, AuthenticationFactory.CommonAdTenant, password, dialog)
                    .AccessToken;

            return new TokenCloudCredentials(accessToken);
        }

        public static SubscriptionCloudCredentials GetSubscriptionCloudCredentials(TokenCloudCredentials creds, Guid subId)
        {
            return new TokenCloudCredentials(subId.ToString(), creds.Token);

        }
    }
}
