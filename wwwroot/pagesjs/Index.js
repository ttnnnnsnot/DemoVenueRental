import { setupLayout, layoutOption } from '../pagesjs/Layout.js';

import indexBannerOption from '../components/IndexBanner.js';
import {
    indexSelectTypeOption,
    indexSelectMore,
    search as searchFunction
} from '../components/IndexSelectType.js';

const appOption = {
    components: {
        // Layout.js
        ...layoutOption.components,
        // Index.js
        'index-select-type': indexSelectTypeOption,
        'index-banner': indexBannerOption,
    },
    setup() {
        // Layout.js
        const { 
            registerComponent,
            loginComponent,
            isLoggedIn,
            layoutLogouted,
            layoutLoggedIn,
            layoutOnBeforeMount,
            layoutOnMounted,
            headerCurrentState,
            checkPathName } = setupLayout();

        onBeforeMount(async () => {
            await layoutOnBeforeMount();

            // index.js
            await indexSelectMoreonBeforeMount();
        });

        onMounted(async () => {
            await layoutOnMounted();

            // index.js
            changeHeaderState();
        });

        const performLoggedIn = async () => {
            await layoutLoggedIn();

            // Index.js
            changeHeaderState();
        }

        const performLogouted = async () => {
            await layoutLogouted();

            // Index.js
            changeHeaderState();
        }

        // Index.js
        const changeHeaderState = () => {
            headerCurrentState.value = isLoggedIn.value ? 2 : 1;
        }


        const fetchData1 = () => API.GET('def/sport');
        const fetchData2 = () => API.GET('def/area');

        const {
            selectTypes,
            fnChangeText,
            onBeforeMount: indexSelectMoreonBeforeMount
        } = indexSelectMore([
            { fetchData: fetchData1, config: "sport" },
            { fetchData: fetchData2, config: "area" }]);

        const search = () => searchFunction(selectTypes);

        // Layout.js
        provide('loggedIn', performLoggedIn);
        provide('logouted', performLogouted);

        return {
            // Layout.js
            registerComponent,
            loginComponent,

            // Index.js
            fnChangeText,
            selectTypes,
            search,

        }
    }
};

const app = createApp(appOption);

app.mount("#app");