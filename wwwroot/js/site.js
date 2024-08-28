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
    nextTick
} = Vue;

// 檢查物件是否為空
const isEmptyObject = (obj) => {
    return Object.keys(obj).length === 0;
};

//const { createVuetify } = Vuetify
//const vuetify = createVuetify()
