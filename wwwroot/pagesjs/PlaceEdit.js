import { setupLayout, layoutOption } from '../pagesjs/Layout.js';

const appOption = {
    components: {
        // Layout.js
        ...layoutOption.components,
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
        });

        onMounted(async () => {
            await layoutOnMounted();
            // PlaceEdit.js
            changeHeaderState();
            const buttons = document.querySelectorAll('#nav-tab > .nav-link');
            activeTabList.value = Array.from(buttons).map(button => `#${button.id}`);
            document.querySelector(activeTabList.value[activeTab.value]).click();
        });

        const performLoggedIn = async () => {
            await layoutLoggedIn();

            // PlaceManage.js
            changeHeaderState();
        }

        const performLogouted = async () => {
            await layoutLogouted();

            // PlaceManage.js
            changeHeaderState();
        }

        // PlaceEdit.js
        const activeTabList = ref([]);
        const activeTab = ref(0);

        const changeHeaderState = () => {
            headerCurrentState.value = isLoggedIn.value ? 3 : 1;
        }

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

        // Layout.js
        provide('loggedIn', performLoggedIn);
        provide('logouted', performLogouted);

        return {
            // Layout.js
            registerComponent,
            loginComponent,

            // PlaceEdit.js
            nextDone,
            upDone
        }
    }
};

const app = createApp(appOption);

app.mount("#app");