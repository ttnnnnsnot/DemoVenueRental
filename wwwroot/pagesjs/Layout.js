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

    const isLoggedIn = ref(null);

    const { checkPathName, onMounted: onMountedLoginSystem, listenerOnLogout } = loginStstem(showLoginModel, isLoggedIn);

    const Logout = async () => {
        try {
            let Token = await API.POST('User/Logout', {});
            isLoggedIn.value = false;
        } catch (error) {
            console.log(error);
        }
    }

    const Logouted = async () => {
        await Logout();
        checkPathName();
        await listenerOnLogout();
    }

    const LoggedIn = async () => {
        isLoggedIn.value = await IsLoggedIn();
    }

    const onBeforeMount = async () => {

        await API.GET('Token');

        if (!isEmptyObject(accessDenied)) {
            Alert.addDanger("您沒有權限!")
        }
    };

    const onMounted = async () => {
        await nextTick();
        isLoggedIn.value = await IsLoggedIn();
        await onMountedLoginSystem();
    };

    return {
        registerComponent, showRegisterModel,
        loginComponent, showLoginModel,

        isLoggedIn,
        Logouted,
        LoggedIn,

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
        Logouted,
        LoggedIn,

        onBeforeMount: LayoutonBeforeMount,
        onMounted: LayoutonMounted } = useRegister();

    const headerCurrentState = ref(1);

    watch(loginComponent, (newVal) => {
        if (!isEmptyObject(noLogin)) {
            showLoginModel();
        }
    });

    provide('isLoggedIn', isLoggedIn);
    provide('currentState', headerCurrentState);

    provide('Logouted', Logouted);
    provide('showRegisterModel', showRegisterModel);
    provide('showLoginModel', showLoginModel);

    provide('checkPathName', checkPathName);

    return {
        registerComponent, showRegisterModel,
        loginComponent, showLoginModel,

        LoggedIn, Logouted,

        LayoutonBeforeMount,
        LayoutonMounted,

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