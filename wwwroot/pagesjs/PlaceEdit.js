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
        const { registerComponent, showRegisterModal,
            loginComponent, showLoginModal,
            Logouted,
            LoggedIn,
            LayoutonBeforeMount,
            LayoutonMounted,
            headerCurrentState } = setupLayout();

        onBeforeMount(async () => {
            await LayoutonBeforeMount();
        });

        onMounted(async () => {
            await LayoutonMounted();
            headerCurrentState.value = 3;
            // PlaceEdit.js
            const buttons = document.querySelectorAll('#nav-tab > .nav-link');
            activeTabList.value = Array.from(buttons).map(button => `#${button.id}`);
            document.querySelector(activeTabList.value[activeTab.value]).click();
        });

        // PlaceEdit.js
        const activeTabList = ref([]);
        const activeTab = ref(0);

        const sendForm = async () => {
            const forms = document.querySelectorAll('#PlaceEditTab form');
            const thisform = forms[activeTab.value];

            if (!$(thisform).valid())
                return false;

            const formData = new FormData(thisform);

            const data = {};
            formData.forEach((value, key) => {
                let keyreplace = key.replace('PlaceInfo.', '');
                if (keyreplace !== 'AntiforgeryToken') { // 排除不需要的欄位
                    data[keyreplace] = value;
                }
            });

            try {
                const results = await API.POST("Place", data);
                console.log(results)
                Alert.addDanger(results);
                return true;

            } catch (error) {
                console.log('Error:', error);
                return false;
            }
        }


        const nextDone = async () => {
            if (await sendForm()) {
                activeTab.value = getIndex();
                if (activeTab.value < activeTabList.value.length - 1) {
                    activeTab.value++;
                    document.querySelector(activeTabList.value[activeTab.value]).click();
                }
            }
        }

        const upDone = () => {
            activeTab.value = getIndex();
            if (activeTab.value > 0) {
                activeTab.value--;
                document.querySelector(activeTabList.value[activeTab.value]).click();
            }
        }

        const getIndex = () => {
            const buttons = document.querySelectorAll('#nav-tab > .nav-link');
            let myIndex = 0;
            buttons.forEach((button, index) => {
                if (button.classList.contains('active')) {
                    myIndex = index;
                }
            });

            return myIndex;
        }

        return {
            // Layout.js
            showRegisterModal, registerComponent,
            loginComponent, showLoginModal,
            LoggedIn,
            Logouted,

            // PlaceEdit.js
            nextDone,
            upDone
        }
    }
};

const app = createApp(appOption);

app.mount("#app");