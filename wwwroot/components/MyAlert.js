const myAlert = () => {
    const alerts = ref([]);

    const generateId = () => '_' + Math.random().toString(36).substring(2, 9); // 簡單的隨機ID生成器

    const add = (message, messageType = 2) => {
        const id = generateId();
        const { alertType, iconType } = changeType(messageType);
        alerts.value.push({ id, message, removing: false, alertType, iconType });
        let index = alerts.value.length - 1;
        setTimeout(() => {
            remove(id);
        }, 3000); // 與動畫持續時間相匹配
    };

    const addExclamation = (message) => { add(message, 0); }
    const addSuccess = (message) => { add(message, 1); }
    const addWarning = (message) => { add(message, 2); }
    const addDanger = (message) => { add(message, 3); }

    const changeType = (messageType) => {
        switch (messageType) {
            case 0:
                return { "alertType": "alert-primary", iconType: "fa-solid fa-circle-exclamation" };
            case 1:
                return { "alertType": "alert-success", iconType: "fa-solid fa-circle-check" };
            case 2:
                return { "alertType": "alert-warning", iconType: "fa-solid fa-triangle-exclamation" };
            case 3:
                return { "alertType": "alert-danger", iconType: "fa-solid fa-triangle-exclamation" };
        }
    }

    const remove = (id) => {
        const alertIndex = alerts.value.findIndex(alert => alert.id === id);
        if (alertIndex !== -1) {
            alerts.value[alertIndex].removing = true; // 設置 `removing` 為 true，觸發 `fadeOut` 動畫
        }
    };

    const onAnimationEnd = (id) => {
        const alertIndex = alerts.value.findIndex(alert => alert.id === id);
        if (alertIndex !== -1 && alerts.value[alertIndex].removing) {
            alerts.value.splice(alertIndex, 1);
        }
    };

    return {
        alerts,
        addExclamation,
        addSuccess,
        addWarning,
        addDanger,
        remove,
        onAnimationEnd
    };
};

const myAlertApp = createApp({
    setup() {

        const { alerts, add, remove, onAnimationEnd,
            addExclamation,
            addSuccess,
            addWarning,
            addDanger, } = myAlert();
        return {
            alerts,
            addExclamation,
            addSuccess,
            addWarning,
            addDanger,
            remove,
            onAnimationEnd
        }
    }
});

const Alert = myAlertApp.mount(".myalert");