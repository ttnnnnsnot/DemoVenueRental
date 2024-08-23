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
            isLoggedIn, currentState, headerLinks, setShowType,
            onMounted: headerOnMounted } = useRegister();

        const fetchData1 = () => fetchWithParams('def/sport');
        const fetchData2 = () => fetchWithParams('def/area');

        const {
            selectTypes,
            fnChangeText,
            onMounted: indexSelectMoreOnMounted
        } = indexSelectMore([fetchData1, fetchData2]);

        const search = () => searchFunction.search(selectTypes);

        const LoggedIn = async () => {
            isLoggedIn.value = await IsLoggedIn();
        }
             
        onMounted(async () => {
            await indexSelectMoreOnMounted();
            await headerOnMounted();
        });

        return {
            isLoggedIn, currentState, headerLinks, setShowType,
            showRegisterModal, registerComponent,
            loginComponent, showLoginModal,

            fnChangeText,
            selectTypes,
            search,

            LoggedIn
        }
    }
};

const app = createApp(appOption);

app.mount("#app");