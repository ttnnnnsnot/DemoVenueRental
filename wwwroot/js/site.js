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

