// 獲取 API 基本 URL
const getBaseUrl = (area) => {
    const protocol = `${location.protocol}//`;
    const host = location.hostname;
    const port = location.port ? `:${location.port}/` : "";
    const areaPath = area ? `${area}/` : "";
    return `${protocol}${host}${port}${areaPath}`;
}

const API = axios.create({
    baseURL: getBaseUrl(),
    headers: {
        "Content-Type": "application/json; charset=utf-8",
        Accept: "application/json",
    },
    timeout: 500 // 設定 5 秒的超時時間
});

// 添加 request 攔截器
API.interceptors.request.use((config) => {
    // 這裡可以獲取請求方法
    if (config.method === 'post' || config.method === 'put' || config.method === 'delete')
    {
        const token = $('input[name="AntiforgeryToken"]').val(); // 取得 CSRF token
        config.headers["X-CSRF-TOKEN"] = token; // 添加 CSRF token 到標頭
    }

    return config;
}, (error) => {
    // 對請求錯誤進行處理
    return Promise.reject(error);
});

API.GET = async (url, params) => {
        try {
            const urlPath = getBaseUrl('api') + url;
            const res = await API.get(urlPath, {
                params,
            });
            return JSON.parse(res.data);
        } catch (res) {
            return Promise.reject(res.message);
        }
}

API.GetTemplate = async (url) => {
    try {
        const res = await API.get(url, {
            responseType: 'text',
        });
        return res.data;
    } catch (res) {
        return Promise.reject(res.message);
    }
}

API.POST = async (url, ...arg) => {
    try {
        const urlPath = getBaseUrl('api') + url;
        const res = await API.post(urlPath, ...arg);
        return JSON.parse(res.data);
    } catch (res) {
        return Promise.reject(res.message);
    }
}

// 確認登入狀態
const IsLoggedIn = async () => {
    try {
        return await API.GET('User/IsLoggedIn');
    } catch (error) {
        console.log(error);
        return false;
    }
}