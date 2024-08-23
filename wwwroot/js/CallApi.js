// 檢查物件是否為空
const isEmptyObject = (obj) => {
    return Object.keys(obj).length === 0;
};

// 確認登入狀態
const IsLoggedIn = () => {
    return fetchWithParams('User/IsLoggedIn');
}

// 獲取 API 基本 URL
const getApiBaseUrl = () => {
    const protocol = location.protocol;
    const host = location.hostname;
    const port = location.port ? `:${location.port}` : "";
    const controller = "api";
    return `${protocol}//${host}${port}/${controller}/`;
}

// 加載模板
const loadTemplate = async (url) => {
    try {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return await response.text();
    } catch (error) {
        console.error('Error loading template:', error);
        throw error;
    }
}

// 帶參數的 fetch 請求
const fetchWithParams = async (url, params, method = 'GET', headers = { 'Content-Type': 'application/json' }) => {
    try {
        let fullUrl = getApiBaseUrl() + url;
        let options = {
            method: method,
            headers: headers // 添加自訂標頭
        };

        if (method === 'GET') {
            // 將參數轉換成 URL 的查詢字串
            fullUrl += '?' + new URLSearchParams(params).toString();
        } else {
            const token = $('input[name="AntiforgeryToken"]').val();
            options.headers["X-CSRF-TOKEN"] = token;
            options.body = JSON.stringify(params); // 確保參數是 JSON 字串
        }

        const response = await fetch(fullUrl, options);

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        return await response.json(); // 成功時返回解析後的資料

    } catch (error) {
        console.error('Error fetching data:', error);
        throw error; // 錯誤時返回錯誤訊息
    }
}
