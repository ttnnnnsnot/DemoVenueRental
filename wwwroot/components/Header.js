const headerOption = () => {
    const isLoggedIn = ref(false);
    const currentState = ref(1); // 初始狀態為state1
    const headerLinks = reactive([false, false, false, false, false, false, false]);

    const setShowType = (state) => {
        switch (state) {
            case 1:
                headerLinks[0] = true;
                headerLinks[1] = true;
                headerLinks[2] = true;
                headerLinks[3] = true;
                break;
            case 2:
                headerLinks[0] = true;
                headerLinks[1] = true;
                headerLinks[2] = false;
                headerLinks[3] = false;
                break;
            case 3:
                headerLinks[0] = false;
                headerLinks[1] = false;
                headerLinks[2] = false;
                headerLinks[3] = false;
                break;
            default:
                // 如果狀態不匹配，則隱藏所有連結
                headerLinks.fill(false);
                break;
        }
    };

    const onMounted = async () => {
        isLoggedIn.value = await IsLoggedIn();
        setShowType(currentState.value);
    };

    // 監聽isLoggedIn的變化
    watch(isLoggedIn, (newValue) => {
        if (!newValue) {
            currentState.value = 1;
        } else {
            currentState.value = 2;
        }
        setShowType(currentState.value);
    });

    // 監聽currentState的變化
    watch(currentState, (newState) => {
        setShowType(newState);
    });

    return {
        isLoggedIn,
        currentState,
        headerLinks,
        setShowType,
        onMounted
    };
};

export default headerOption;