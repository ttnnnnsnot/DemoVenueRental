import { useRegister, layoutOption } from '../pagesjs/Layout.js';

import indexBannerOption from '../components/IndexBanner.js';
import {
    indexSelectTypeOption,
    indexSelectMore,
    search as searchFunction
} from '../components/IndexSelectType.js';



const appOption = {
    components: {
        ...layoutOption.components,
        'index-select-type': indexSelectTypeOption,
        'index-banner': indexBannerOption,
    },
    setup() {
        const { registerComponent, showRegisterModal,
            loginComponent, showLoginModal,
            Logout } = useRegister();

        const isLoggedIn = ref(null);

        const fetchData1 = () => API.GET('def/sport');
        const fetchData2 = () => API.GET('def/area');

        const {
            selectTypes,
            fnChangeText,
            onBeforeMount: indexSelectMoreonBeforeMount
        } = indexSelectMore([fetchData1, fetchData2]);

        const search = () => searchFunction(selectTypes);

        const Logouted = async () => {
            await Logout();
            isLoggedIn.value = await IsLoggedIn();
        }

        const LoggedIn = async () => {
            isLoggedIn.value = await IsLoggedIn();
        }
        
        onBeforeMount(async () => {
            isLoggedIn.value = await IsLoggedIn();
            await indexSelectMoreonBeforeMount();
        });

        onMounted(async () => {
            await nextTick();
            if (!isEmptyObject(accessDenied)) {
                Alert.addDanger("您沒有權限!")
            }
        });

        watch(loginComponent, (newVal) => {
            if (!isEmptyObject(noLogin)) {
                showLoginModal();
            }
        });



        provide('isLoggedIn', isLoggedIn);

        return {
            showRegisterModal, registerComponent,
            loginComponent, showLoginModal,

            fnChangeText,
            selectTypes,
            search,

            LoggedIn,
            Logouted
        }
    }
};

const app = createApp(appOption);

app.mount("#app");