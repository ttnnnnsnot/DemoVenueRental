const {
    ref,
    createApp,
    reactive,
    onMounted,
    defineAsyncComponent,
    watch,
    provide,
    inject,
    onBeforeMount,
    onUpdated,
    onBeforeUpdate,
    nextTick,
    getCurrentInstance
} = Vue;

// 檢查物件是否為空
const isEmptyObject = (obj) => {
    return Object.keys(obj).length === 0;
};

const trimString = (str) => {
    return str.replace(/^\s+|\s+$/g, '');
}

// 確認 DOM 已完成載入
const waitForDOMLoad = (timeout = 1000) => {
    return new Promise((resolve, reject) => {
        let observer;
        let timer;

        try {
            observer = new MutationObserver((mutations, obs) => {
                if (document.readyState === 'interactive') {
                    clearTimeout(timer);
                    resolve();
                    obs.disconnect(); // 停止觀察
                }
            });

            observer.observe(document, {
                childList: true,
                subtree: true
            });

            // 添加超時機制
            timer = setTimeout(() => {
                observer.disconnect(); // 停止觀察
                reject(new Error('DOM load timeout'));
            }, timeout);

            // 如果在超時之前 DOM 已經加載完成，清除計時器
            if (document.readyState === 'complete') {
                clearTimeout(timer);
                resolve();
                observer.disconnect(); // 停止觀察
            }
        } catch (error) {
            if (timer) clearTimeout(timer);
            if (observer) observer.disconnect(); // 停止觀察
            reject(error);
        }
    });
}


const setVueGlobal = (instance, key, value) => {

    if (!instance)
        return;

    const globalProperties = instance.appContext.config.globalProperties;

    // 初始化 $Model 命名空間，如果它還不存在
    if (!globalProperties.$Model) {
        globalProperties.$Model = {};
    }
    // 設置全局方法
    globalProperties.$Model[key] = value;
};

const getVueGlobal = (instance) => {

    if (!instance)
        return;

    const globalProperties = instance.appContext.config.globalProperties;
    return globalProperties.$Model;
};
