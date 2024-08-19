function GetUrl() {
    let protocol = location.protocol;
    let host = location.hostname;
    let port = location.port != null ? `:${location.port}` : "";
    let controll = "api";
    return `${protocol}//${host}${port}/${controll}/`;
}

async function loadTemplate(url) {
    const response = await fetch(url);
    if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
    }
    return await response.text();
}

function fetchWithParams(url, params, method = 'GET', headers = {}) {
    return new Promise(async (resolve, reject) => {
        try {
            let urlfull = GetUrl() + url;
            let options = {
                method: method,
                headers: headers // 添加自訂標頭
            };

            if (method === 'GET') {
                // 將參數轉換成 URL 的查詢字串
                urlfull += '?' + new URLSearchParams(params).toString();
            } else {
                // 構建 FormData 用於傳送檔案和其他資料
                const formData = new FormData();
                for (const key in params) {
                    formData.append(key, params[key]);
                }
                options.body = formData;
            }
            
            const response = await fetch(urlfull, options);

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const data = await response.json();
            resolve(data);  // 成功時返回解析後的資料

        } catch (error) {
            reject(error);  // 錯誤時返回錯誤訊息
        }
    });
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}