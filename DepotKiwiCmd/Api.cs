using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using DepotKiwi.Models;
using DepotKiwi.RequestModels;
using DepotKiwiCmd.Utils;

namespace DepotKiwiCmd {
    internal class Api {
        internal Api(string api) {
            _requestHelper = new RequestHelper(api);
        }
        
        internal StatusResponse CreateRepository(string name) {
            return _requestHelper.Post<StatusResponse, DepotCreateRequest>("depot/create", new DepotCreateRequest {
                Name = name
            });
        }
        
        internal StatusResponse DeleteRepository(string id) {
            return _requestHelper.Post<StatusResponse, DepotDeleteRequest>("depot/delete", new DepotDeleteRequest {
                Id = id
            });
        }

        internal List<DepotListResponse> ListRepositories() {
            return _requestHelper.Get<List<DepotListResponse>>("depot/list", new());
        }

        internal Depot GetDepot(string id) {
            var response = _requestHelper.Get<DepotInfoResponse>($"depot/info/{id}", new());

            return response.Success ? response.Depot : null;
        }

        internal byte[] DownloadFile(string depotId, string file) {
            var buffer = _requestHelper.GetRaw($"depot/file/download/{depotId}/{file}", new(), out var status);

            return status == HttpStatusCode.OK ? buffer : null;
        }

        internal StatusResponse DeleteFile(string depotId, string file) {
            return _requestHelper.Post<StatusResponse>($"depot/file/delete/{depotId}/{file}", new());
        }

        internal StatusResponse UploadFile(string depotId, string name, byte[] buffer) {
            return _requestHelper.Post<StatusResponse, DepotFileUploadRequest>($"depot/file/upload/{depotId}/{name}", new() {
                Data = Convert.ToBase64String(buffer)
            });
        }

        private readonly RequestHelper _requestHelper;
    }
}