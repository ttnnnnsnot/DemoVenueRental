import registerOption from '../components/Register.js';
import loginOption from '../components/Login.js';
import headerTemplate from '../components/Header.js';
import loginStstem from '../js/LoginSystem.js';

const useRegister = () => {

    const registerComponent = ref(null);
    const showRegisterModel = () => {
        if (registerComponent.value) {
            registerComponent.value.showModel();
        }
    };

    const loginComponent = ref(null);
    const showLoginModel = () => {
        if (loginComponent.value) {
            loginComponent.value.showModel();
        }
    };

    const isShowLoginModel = ref(null);
    const isLoggedIn = ref(null);

    const { checkPathName, onMounted: onMountedLoginSystem } = loginStstem(isLoggedIn, isShowLoginModel);

    const logout = async () => {
        try {
            await API.POST('User/Logout', {});
        } catch (error) {
            console.log(error);
        } finally {
            isLoggedIn.value = false;
            API.csrfToken = null;
        }
    }

    const logouted = async () => {
        await logout();
        checkPathName();
    }

    const loggedIn = async () => {
        isLoggedIn.value = await IsLoggedIn();
        API.csrfToken = null;
    }

    const onBeforeMount = async () => {
        await updateToken();
        if (!isEmptyObject(accessDenied)) {
            Alert.addDanger("您沒有權限!")
        }
    };

    const onMounted = async () => {
        try {
            await waitForDOMLoad();
        } catch (error) {
            console.error('DOM load error:', error);
        }
        isLoggedIn.value = await IsLoggedIn();        
        await onMountedLoginSystem();
    };

    // 監聽 isShowLoginModel 的變化
    watch(isShowLoginModel, (newVal) => {
        if (newVal && loginComponent.value) {
            showLoginModel();
            isShowLoginModel.value = false;
        }
    });

    // 監聽 loginComponent 的變化
    watch(loginComponent, (newVal) => {
        if (!isEmptyObject(noLogin)) {
            showLoginModel();
        } else if (newVal && isShowLoginModel.value) {
            showLoginModel();
            isShowLoginModel.value = false;
        }
    });

    return {
        registerComponent, showRegisterModel,
        loginComponent, showLoginModel,

        isLoggedIn,
        isShowLoginModel,
        logouted,
        loggedIn,

        onBeforeMount,
        onMounted,

        checkPathName
    };
}

export const setupLayout = () => {
    // Layout.js
    const {
        checkPathName,

        registerComponent, showRegisterModel,
        loginComponent, showLoginModel,

        isLoggedIn,
        logouted : layoutLogouted,
        loggedIn : layoutLoggedIn,

        onBeforeMount: layoutOnBeforeMount,
        onMounted: layoutOnMounted
    } = useRegister();

    const headerCurrentState = ref(null);

    provide('isLoggedIn', isLoggedIn);
    provide('currentState', headerCurrentState);

    provide('showRegisterModel', showRegisterModel);
    provide('showLoginModel', showLoginModel);

    provide('checkPathName', checkPathName);

    return {
        registerComponent,
        loginComponent,

        isLoggedIn,
        layoutLogouted,
        layoutLoggedIn,

        layoutOnBeforeMount,
        layoutOnMounted,

        headerCurrentState,

        checkPathName
    };
}


export const layoutOption = {
    components: {
        'register-module': registerOption,
        'login-module': loginOption,
        'header-module': headerTemplate
    }
}