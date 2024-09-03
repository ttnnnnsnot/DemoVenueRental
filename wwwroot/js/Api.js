const handleApiError = (error) => {
    return Promise.reject({ state: false, message: error.message });
}

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
    timeout: 800 // 設定 5 秒的超時時間
});

// 添加 request 攔截器
API.interceptors.request.use((config) => {
    // 這裡可以獲取請求方法
    if (config.method === 'post' || config.method === 'put' || config.method === 'delete')
    {
        config.headers["RequestVerificationToken"] = getCookie("RequestVerificationToken"); // 添加 CSRF token 到標頭
    }

    return config;
}, (error) => {
    // 對請求錯誤進行處理
    return handleApiError(error);
});

// 添加 response 攔截器 狀態碼500的錯誤處理
API.interceptors.response.use((response) => {
    return response;
}, (error) => {
    if (error.response) {
        switch (error.response.status) {
            case 400:
                error.message = '請求錯誤';
                break;
            case 401:
                error.message = '未授權，請重新登入';
                break;
            case 403:
                error.message = '拒絕訪問';
                break;
            case 404:
                error.message = '請求地址出錯';
                break;
            case 500:
                error.message = '伺服器錯誤';
                break;
            default:
                error.message = `連接錯誤 ${error.response.status}`;
        }
    } else {
        error.message = '無法連接到伺服器';
    }
    return handleApiError(error);
});

API.GET = async (url, params) => {
    try {
        const urlPath = getBaseUrl('api') + url;
        const res = await API.get(urlPath, {
            params,
        });
        return JSON.parse(res.data);
    } catch (error) {
        return handleApiError(error);
    }
}

API.GetTemplate = async (url) => {
    try {
        const res = await API.get(url, {
            responseType: 'text',
        });
        return res.data;
    } catch (error) {
        return handleApiError(error);
    }
}

API.POST = async (url, ...arg) => {
    try {
        const urlPath = getBaseUrl('api') + url;
        const res = await API.post(urlPath, ...arg);
        return JSON.parse(res.data);
    } catch (error) {
        return handleApiError(error);
    }
    finally {
        await API.GET('Token');
    }
}

// 確認登入狀態
const IsLoggedIn = async () => {
    try {
        return await API.GET('User/IsLoggedIn');
    } catch (error) {
        return false;
    }
}

// 確認是否有權限
const getRole = async (role = "Admin") => {
    try {
        return await API.GET(`User/CheckRole/${role}`);
    } catch (error) {
        return false;
    }
}


function getCookie(name) {
    let cookieValue = null;
    if (document.cookie && document.cookie !== '') {
        const cookies = document.cookie.split(';');
        for (let i = 0; i < cookies.length; i++) {
            const cookie = cookies[i].trim();
            if (cookie.substring(0, name.length + 1) === (name + '=')) {
                cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                break;
            }
        }
    }
    return cookieValue;
}